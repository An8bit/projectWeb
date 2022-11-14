using demoweb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace demoweb.Controllers
{
    public class HomeController : Controller
    {
        private DBSportStoreEntities db = new DBSportStoreEntities();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult TrangChu()
        {
               
                var products = db.Products;
                return View(products.ToList());
            
        }
        
    }
}