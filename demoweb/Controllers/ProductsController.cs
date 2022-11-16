using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using demoweb.Models;
using System.IO;
using EntityState = System.Data.Entity.EntityState;

namespace demoweb.Controllers
{
    public class ProductsController : Controller
    {
        private DBSportStoreEntities db = new DBSportStoreEntities();

        // GET: Products
        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.Category1);
            return View(products.ToList());
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.Category = new SelectList(db.Categories, "IDCate", "NameCate");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include ="ProductID,NamePro,DecriptionPro,Category,Price,ImagePro")] Product product,HttpPostedFileBase ImagePro)
        {
            if (ModelState.IsValid)
            {
                if (ImagePro != null)
                {
                    //Lấy tên file của hình được up lên

                    var fileName = Path.GetFileName(ImagePro.FileName);

                    //Tạo đường dẫn tới file

                    var path = Path.Combine(Server.MapPath("~/Images"), fileName);
                    //Lưu tên

                    product.ImagePro = fileName;
                    //Save vào Images Folder
                    ImagePro.SaveAs(path);

                }
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Category = new SelectList(db.Categories, "IDCate", "NameCate",
            product.Category);
            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.Category = new SelectList(db.Categories, "IDCate", "NameCate", product.Category);
            return View(product);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include ="ProductID,NamePro,DecriptionPro,Category,Price,ImagePro")] Product product,HttpPostedFileBase ImagePro)
        {
            if (ModelState.IsValid)
            {
                var productDB = db.Products.FirstOrDefault(p => p.ProductID ==
                product.ProductID);
                if (productDB != null)
                {
                    productDB.NamePro = product.NamePro;
                    productDB.DecriptionPro = product.DecriptionPro;
                    productDB.Price = product.Price;
                    if (ImagePro != null)
                    {
                        //Lấy tên file của hình được up lên
                        var fileName = Path.GetFileName(ImagePro.FileName);
                        //Tạo đường dẫn tới file
                        var path = Path.Combine(Server.MapPath("~/Images"),fileName);
                        //Lưu tên
                        productDB.ImagePro = fileName;
                        //Save vào Images Folder
                        ImagePro.SaveAs(path);
                    }
                    productDB.Category = product.Category;
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Category = new SelectList(db.Categories, "IDCate", "NameCate",
            product.Category);
            return View(product);
        }
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        // public ActionResult
        // GET: Products
        public ActionResult ProductList(int? page, string category, string SearchString, double min = double.MinValue, double max = double.MaxValue)
        { // Tạo Products và có tham chiếu đến Category
            var products = db.Products.Include(p => p.Category1);
            // Tìm kiếm chuỗi truy vấn theo category
            if (category == null)
            {
                products = db.Products.OrderByDescending(x => x.NamePro);
            }
            else
            {
                products = db.Products.OrderByDescending(x => x.Category).Where(x => x.Category == category);

            }
            //Tìm kiếm chuỗi truy vấn theo NamePro, nếu chuỗi truy vấn SearchString khác rỗng, null
            if (!String.IsNullOrEmpty(SearchString))
            {
                products = products.Where(s => s.NamePro.Contains(SearchString));
            }
            // Tìm kiếm chuỗi truy vấn theo đơn giá
            if (min >= 0 && max > 0)
            {
                products = db.Products.OrderByDescending(x => x.Price).Where(p => (double)p.Price >= min && (double)p.Price <= max);
            }

            // Khai báo mỗi trang 4 sản phẩm
            int pageSize = 4;
            // Toán tử ?? trong C# mô tả nếu page khác null thì lấy giá trị page, còn
            // nếu page = null thì lấy giá trị 1 cho biến pageNumber.
            int pageNumber = (page ?? 1);
            // Nếu page = null thì đặt lại page là 1.
            if (page == null) page = 1;
            // Trả về các product được phân trang theo kích thước và số trang.
            return View(products.ToList());
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
