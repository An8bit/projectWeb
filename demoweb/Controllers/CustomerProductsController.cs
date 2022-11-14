using demoweb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace demoweb.Controllers
{
    public class CustomerProductsController : Controller
    {
        private DBSportStoreEntities db = new DBSportStoreEntities();
        // GET: Products
        public ActionResult Index()
        {
            var products = db.Products;
            return View(products.ToList());
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new
                HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }
       
        
       
    }
}

