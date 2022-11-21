using demoweb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading;

namespace demoweb.Controllers
{
    public class UsersController : Controller
    {
        private DBSportStoreEntities database = new DBSportStoreEntities();
        // GET: Users
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
       
        [HttpPost]
        public ActionResult Register(Customer cust)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(cust.NameCus))
                    ModelState.AddModelError(string.Empty, "Tên đăng nhập không được để trống");
            if (string.IsNullOrEmpty(cust.PassCus))
                    ModelState.AddModelError(string.Empty, "Mật khẩu không được để trống");
            if (string.IsNullOrEmpty(cust.EmailCus))
                    ModelState.AddModelError(string.Empty, "Email không được để trống");
            if (string.IsNullOrEmpty(cust.PhoneCus))
                    ModelState.AddModelError(string.Empty, "Điện thoại không được để trống");
                    //Kiểm tra xem có người nào đã đăng kí với tên đăng nhập này hay chưa
                    var khachhang = database.Customers.FirstOrDefault(k =>
                    k.NameCus == cust.NameCus);
                if (khachhang != null)
                    ModelState.AddModelError(string.Empty, "Đã có người đăng kí tên này");
            if (ModelState.IsValid)
                {
                    database.Customers.Add(cust);
                    database.SaveChanges();
                }
                else
                {
                    return View();
                }
            }
            return RedirectToAction("Login");
        }
        [HttpPost]
        public ActionResult Login(Customer cust)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(cust.NameCus))
                    ModelState.AddModelError(string.Empty, "Tên đăng nhập không được để trống");
                if (string.IsNullOrEmpty(cust.PassCus))
                    ModelState.AddModelError(string.Empty, "Mật khẩu không được để trống");
                if(ModelState.IsValid)
               
                {
                    //tìm khách hàng có tên đăng nhập và password hợp lệ trong csdl
                    var khachhang = database.Customers.FirstOrDefault(k => k.NameCus == cust.NameCus && k.PassCus == cust.PassCus);
                    var admin = database.AdminUsers.FirstOrDefault(k=>k.NameUser==cust.NameCus && k.PasswordUser==cust.PassCus);
                    if(khachhang != null)
                    {
                        
                        //lưu vào sesion
                        Thread.Sleep(2000);
                        Session["TaiKhoan"] = khachhang.NameCus;
                        ViewBag.ThongBao = "Chúc mừng đăng nhập thành công";
                        return RedirectToAction("ProductList", "Products");

                    }else if(admin != null)
                    {
                        Session["admin"] = admin.NameUser;

                        return RedirectToAction("Index", "Categories");
                    }
                    else
                    {
                        ViewBag.ThongBao = "Tên đang nhập hoặc mật khẩu không chính xác";
                    }
                }
                
            }
            
            return View();
        }
        public ActionResult Logout()
        {
            if (Session["taikhoan"] != null)
            {
                Session.Abandon();
                return RedirectToAction("TrangChu", "Home");

            }
            else if (Session["admin"] != null)
            {
                Session.Abandon();
                return RedirectToAction("TrangChu", "Home");
            }

            return RedirectToAction("login", "User");
        }
    }
}
