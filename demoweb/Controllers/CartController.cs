using demoweb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace demoweb.Controllers
{
    public class CartController : Controller
    {
        private DBSportStoreEntities database = new DBSportStoreEntities();
        // GET: Cart
        public List<CartItem> GetCart()
        {
           
            
                List<CartItem> myCart = Session["GioHang"] as
                List<CartItem>;
                //Nếu giỏ hàng chưa tồn tại thì tạo mới và đưa vào Session
                if (myCart == null)
                {
                    myCart = new List<CartItem>();
                    Session["GioHang"] = myCart;
                }
                return myCart;
            
           
        }
        public ActionResult AddToCart(int id)
        {
            //Lấy giỏ hàng hiện tại
            List<CartItem> myCart = GetCart();
            if (Session["TaiKhoan"] != null)
            {
                CartItem currentProduct = myCart.FirstOrDefault(p => p.ProductID == id);
                if (currentProduct == null)
                {
                    currentProduct = new CartItem(id);
                    myCart.Add(currentProduct);
                }
                else
                {
                    currentProduct.Number++; //Sản phẩm đã có trong giỏ thì tăng số lượng lên 1
                }
                return RedirectToAction("ProductList", "Products", new
                {
                    id = id
                });
            }
            return RedirectToAction("Login", "Users");
        }
        private int GetTotalNumber()
        {
            int totalNumber = 0;
            List<CartItem> myCart = GetCart();
            if (myCart != null)
                totalNumber = myCart.Sum(sp => sp.Number);
            return totalNumber;
        }
        private decimal GetTotalPrice()
        {
            decimal totalPrice = 0;
            List<CartItem> myCart = GetCart();
            if (myCart != null)
                totalPrice = myCart.Sum(sp => sp.FinalPrice());
            return totalPrice;
        }
        public ActionResult GetCartInfo()
        {
            List<CartItem> myCart = GetCart();
            //Nếu giỏ hàng trống thì trả về trang ban đầu
            if (myCart == null || myCart.Count == 0)
            {
                return RedirectToAction("ProductList", "Products");
            }
            ViewBag.TotalNumber = GetTotalNumber();
            ViewBag.TotalPrice = GetTotalPrice();
            return View(myCart); //Trả về View hiển thị thông tin giỏ hàng

        }
        //hàm xóa sản phẩm trong giỏ hàng
        public ActionResult Remove(int id)
        {
            List<CartItem> myCart = GetCart();
            myCart.RemoveAll(s => s.ProductID == id);

            return RedirectToAction("GetCartInfo", "Cart");
        }
        //TONG SL HANG
        public ActionResult CartPartial()
        {
            ViewBag.TotalNumber = GetTotalNumber();
            ViewBag.TotalPrice = GetTotalPrice();
            return PartialView();
        }
        //XÓA GIỎ HÀNG
        public ActionResult ClearCart()
        {
            if (Session["TaiKhoan"] != null)
            {
                List<CartItem> myCart = GetCart();
                myCart.Clear();
                return RedirectToAction("GetCartInfo", "Cart");
            }
            else
            {
                return RedirectToAction("Login", "Users");
            }
           
        }
        //edit số hàng thêm hoặc bớt
        public ActionResult update_quatity(FormCollection form)
        {
            List<CartItem> items = GetCart();
            int id = int.Parse(form["Idpro"]);
            var item = items.Find(s => s.ProductID == id);
            int a = int.Parse(form["quantity"]);
            if (a < 0)
            {
                return RedirectToAction("GetCartInfo", "Cart");
            }
            else
            {
                if (item != null)
                {
                    item.Number = a;
                }
            }

            return RedirectToAction("GetCartInfo", "Cart");
        }

     /*   public ActionResult CheckOut(FormCollection form)
        {
            try
            {
                List<CartItem> myCart = GetCart();

                OrderPro oder = new OrderPro();
                oder.DateOrder = DateTime.Now;
                oder.AddressDeliverry = form["AddressDeliverry"];
                oder.IDCus = int.Parse(form["ID"]);
                database.OrderProes.Add(oder);
                foreach (var item in myCart)
                {
                    OrderDetail detail = new OrderDetail();
                    detail.IDOrder = oder.ID;
                    detail.IDProduct = item.ProductID;
                    detail.UnitPrice = (double)item.Price;
                    detail.Quantity = item.Number;
                    database.OrderDetails.Add(detail);
                }
                database.SaveChanges();
                myCart.Clear();
                return RedirectToAction("checking_sucess", "Cart");

            }
            catch
            {
                return Content("error checkout");

            }
           
        }
        public ActionResult checking_sucess()
        {
            return View();
        }*/
    }
}