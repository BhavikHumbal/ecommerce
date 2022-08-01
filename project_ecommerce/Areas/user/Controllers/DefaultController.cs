using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using project_ecommerce.EDM;
using project_ecommerce.Models;

namespace project_ecommerce.Areas.user.Controllers
{
    public class DefaultController : Controller
    {
        EcommerceEntities1 e = new EcommerceEntities1();
        // GET: user/Default
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult login(FormCollection fc)
        {
            string email = fc["email"];
            string pass = fc["password"];

            var userobj = e.customers.Where(u => u.email == email && u.password == pass).FirstOrDefault();
            if (userobj != null)
            {
                Session["userID"] = userobj.customer_id;
                Session["username"] = userobj.fname;
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.loginerror = "invalid email or password";
            }
            return View();
        }
        public ActionResult logout()
        {
            Session.Abandon();
            Session.Clear();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult registration()
        {
            return View();
        }
        [HttpPost]
        public ActionResult registration(customer obj)
        {
            e.customers.Add(obj);
            e.SaveChanges();
            return RedirectToAction("login");
        }

        public ActionResult Products()
        {
            return View(e.products.ToList());
        }

        public ActionResult Single(int id)
        {
            return View(e.products.Find(id));
        }

        public string AddToCart(int id)
        {
            int userid = Convert.ToInt32(Session["userID"]);
            var cartobj = e.cards.Where(x => x.product_id == id && x.customer_id == userid).FirstOrDefault();

            if (cartobj != null)
            {
                cartobj.quantity += 1;

                e.Entry(cartobj).State = System.Data.Entity.EntityState.Modified;
                e.SaveChanges();
            }
            else
            {
                card obj = new card();
                obj.product_id = id;
                obj.customer_id = userid;
                obj.quantity = 1;

                e.cards.Add(obj);
                e.SaveChanges();
            }
            return "product add to cart";
        }

        public ActionResult Cart()
        {
            int userid = Convert.ToInt32(Session["userID"]);
            return View(e.cards.Where(c => c.customer_id == userid).ToList());
        }

        [HttpPost]
        public string UpdateCartQty(int id, int qty)
        {
            var obj = e.cards.Find(id);
            obj.quantity = qty;

            e.Entry(obj).State = System.Data.Entity.EntityState.Modified;
            e.SaveChanges();

            return "Cart Qty updated.";
        }

        public ActionResult Delete(int id)
        {
            return View(e.cards.Find(id));
        }
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult Deletehb(int id)
        {
            e.cards.Remove(e.cards.Find(id));
            e.SaveChanges();
            return RedirectToAction("Cart");
        }
        public ActionResult Checkout()
        {
            int userid = Convert.ToInt32(Session["UserId"]);
            Eorder obj = new Eorder();
            obj.order_data = DateTime.Now;
            obj.order_status = Enumstatus.Confirmed.ToString();
            obj.customer_id = userid;

            e.Eorders.Add(obj);
            e.SaveChanges();

            order_detail objdetails = new order_detail();
            var cart = e.cards.Where(c => c.customer_id == userid).ToList();

            objdetails.order_id = obj.order_id;
            double? CartTotal = 0;
            foreach (var item in cart)
            {
                CartTotal += (item.product.cost - (item.product.cost * item.product.discount / 100)) * item.quantity;
                objdetails.quantity = item.quantity;
                objdetails.product_id = item.product_id;
                e.order_detail.Add(objdetails);
                e.SaveChanges();
            }
            return RedirectToAction("Success");
        }

        public ActionResult Success()
        {
            return View();
        }

        public ActionResult Failed()
        {
            return View();
        }
    }

}