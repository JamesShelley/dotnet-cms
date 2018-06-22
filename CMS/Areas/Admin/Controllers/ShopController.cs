using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CMS.Models.Data;
using CMS.Models.ViewModels.Shop;

namespace CMS.Areas.Admin.Controllers
{
    public class ShopController : Controller
    {
        // GET: Admin/Shop/Categories
        public ActionResult Categories()
        {
            // Declare a list of Models

            List<CategoryVM> categoryVMList;

            using (Db db = new Db())
            {
                // Initialize the list
                categoryVMList = db.Categories
                    .ToArray()
                    .OrderBy(x => x.Sorting)
                    .Select(x => new CategoryVM(x))
                    .ToList();
            }

            // Return the view with list
            return View(categoryVMList);
        }
    }
}