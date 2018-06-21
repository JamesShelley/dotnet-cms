using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CMS.Models.Data;
using CMS.Models.ViewModels.Pages;

namespace CMS.Areas.Admin.Controllers
{
    public class PagesController : Controller
    {
        // GET: Admin/Pages
        public ActionResult Index()
        {
            // Declare a list of PageVM
            List<PageVM> pagesList;

            using (Db db = new Db())
            {
                //Init the list
                pagesList = db.Pages.ToArray().OrderBy(x => x.Sorting).Select(x => new PageVM(x)).ToList();
            }
            //Return the view with the list
            return View(pagesList);
        }

        // GET: Admin/Pages/AddPage
        [HttpGet]
        public ActionResult AddPage()
        {
            return View();
        }

        // Post: Admin/Pages/AddPage
        [HttpPost]
        public ActionResult AddPage(PageVM model)
        {
            // Check Mode state
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (Db db = new Db())
            {

                // Declare slug
                string slug;
                //Init PageDTO
                PageDTO dto = new PageDTO();
               
                // DTO title
                dto.Title = model.Title;
                // Check for and set slug
                if (string.IsNullOrWhiteSpace(model.Slug))
                {
                    slug = model.Title.Replace(" ", "-").ToLower();
                }
                else
                {
                    slug = model.Slug.Replace(" ", "-").ToLower();

                }
                // Make sure title and slug are unique
                if (db.Pages.Any(x => x.Title == model.Title) || db.Pages.Any(x => x.Slug == slug))
                {
                    ModelState.AddModelError("","That title or slug already exists.");
                    return View(model);
                }
                // DTO the rest
                dto.Slug = slug;
                dto.Body = model.Body;
                dto.hasSidebar = model.HasSidebar;
                dto.Sorting = 100;

                // Save DTO
                db.Pages.Add(dto);
                db.SaveChanges();

            }

            // Set TempData message
            TempData["SM"] = "You have added a new page!";
            //Redirect
            return RedirectToAction("AddPage");

        }

        // GET: Admin/Pages/EditPage/id
        [HttpGet]
        public ActionResult EditPage(int id)
        {   
            // Declare PageVM
            PageVM model;

            using (Db db = new Db())
            {

                // Get the page
                PageDTO dto = db.Pages.Find(id);
                // Confirm the page exists
                if (dto == null)
                {
                    return Content("The page does not exist");
                } 
                // Initialize PageVM
                model = new PageVM(dto);

            }
            // Return view with model
            return View(model);
        }
        
        // POST: Admin/Pages/EditPage/id
        [HttpPost]
        public ActionResult EditPage(PageVM model)
        {
            // Check the Model state
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (Db db = new Db())
            {

                // Get the Page id
                int id = model.Id;
                // init slug
                string slug = "home";
                // Get the Page
                PageDTO dto = db.Pages.Find(id);
                // DTO the title
                dto.Title = model.Title;
                // Check for slug and set it if needed
                if (model.Slug != "home")
                {
                    if (string.IsNullOrWhiteSpace(model.Slug))
                    {
                        slug = model.Title.Replace(" ", "-").ToLower();
                    }
                    else
                    {
                        slug = model.Slug.Replace(" ", "-").ToLower();

                    }
                }
                // Make sure the title and the slug are unique
                if (db.Pages.Where(x => x.Id != id).Any(x => x.Title == model.Title) ||
                    db.Pages.Where(x => x.Id != id).Any(x => x.Slug == model.Slug))
                {
                    ModelState.AddModelError("", "That title or slug already exists.");
                    return View(model);
                }
                

                //DTO thet rest
                dto.Slug = slug;
                dto.Body = model.Body;
                dto.hasSidebar = model.HasSidebar;

                // Save the DTO
                db.SaveChanges();

            }
            // Set TempData message
            TempData["SM"] = "You have edited the page!";
      
            // Redirect
            return RedirectToAction("EditPage");
        }

        // GET : Admin/Pages/PageDetails/id
        public ActionResult PageDetails(int id)
        {

            //Declare PagVM
            PageVM model;

            using (Db db = new Db())
            {
                // GET the page
                PageDTO dto = db.Pages.Find(id);
                // Confirm the page exists
                if (dto == null)
                {
                    return Content("The page does not exist");
                }
                
                // Init the PageVM
                model = new PageVM(dto);
            }
            // Return the view with model
            return View(model);
        }

        // GET : Admin/Pages/DeletePage/id
        public ActionResult DeletePage(int id)
        {
            using (Db db = new Db())
            {

                // GET the page
                PageDTO dto = db.Pages.Find(id);
                //Remove the page
                db.Pages.Remove(dto);
                //Save 
                db.SaveChanges();
            }
            //Redirect
            return RedirectToAction("index");
        }

        [HttpPost]
        // POST: Admin/Pages/ReorderPages
        public void ReorderPages(int[] id)
        {

            using (Db db = new Db())
            {

                // Set initial Count
                int count = 1;
                // Declare PageDTO
                PageDTO dto;
                // Set sorting for each page
                foreach (var pageId in id)
                {
                    dto = db.Pages.Find(pageId);
                    dto.Sorting = count;

                    db.SaveChanges();

                    count++;
                }
            }

            // return
        }

        [HttpGet]
        // GET : Admin/Pages/EditSidebar
        public ActionResult EditSidebar()
        {

            //Declare Model
            SidebarVM model;
            using (Db db = new Db())
            {
                // GET the DTO
                SidebarDTO dto = db.Sidebar.Find(1);
                
                // init the Model
                model = new SidebarVM(dto);
            }

            // Return view with model
            return View(model);
        }

        [HttpPost]
        // POST : Admin/Pages/EditSidebar
        public ActionResult EditSideBar(SidebarVM model)
        {

            using (Db db = new Db())
            {

                // Get the DTO
                SidebarDTO dto = db.Sidebar.Find(1);
                // DTO the body
                dto.Body = model.Body;
                // Save
                db.SaveChanges();
            }

            // Set template message
            TempData["SM"] = "You have editted the Sidebar";
            // Redirect

            return RedirectToAction("EditSideBar");
        }

    }
}