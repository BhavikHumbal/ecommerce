using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using project_ecommerce.EDM;

namespace project_ecommerce.Areas.Admin.Controllers
{
    public class hbController : Controller
    {
        EcommerceEntities1 e = new EcommerceEntities1();
        // GET: Admin/hb
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            return View(e.products.Find(id));
        }
        [HttpPost]
        public ActionResult Edit(product obj, HttpPostedFileBase product_img1)
        {
            if (product_img1 != null)
            {
                string filename = Path.GetFileName(product_img1.FileName);
                string fullpath = Path.Combine(Server.MapPath("~/Docs"), filename);
                product_img1.SaveAs(fullpath);
                obj.product_img1 = filename;


            }

            e.Entry(obj).State = System.Data.Entity.EntityState.Modified;
            e.SaveChanges();
            return RedirectToAction("ManageProducts");
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            return View(e.products.Find(id));
        }
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult Deletehb(int id)
        {
            e.products.Remove(e.products.Find(id));
            e.SaveChanges();
            return RedirectToAction("ManageProducts");
        }

        public ActionResult login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult login(FormCollection fc)
        {
            string email = fc["email"];
            string pass = fc["password"];

            var Adminobj = e.admins.Where(u => u.email == email && u.password == pass).FirstOrDefault();
            if (Adminobj != null)
            {
                Session["userID"] = Adminobj.admin_id;
                Session["username"] = Adminobj.fname;
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
            return RedirectToAction("login");
        }

        public ActionResult ManageProducts()
        {
            return View(e.products.ToList());
        }
        [HttpGet]
        public ActionResult addproduct()
        {
            return View();
        }
        [HttpPost]
        public ActionResult addproduct(product obj , HttpPostedFileBase product_img1)
        {
            if(product_img1 != null)
            {
                string filename = Path.GetFileName(product_img1.FileName);
                string fullpath = Path.Combine(Server.MapPath("~/Docs"), filename);
                product_img1.SaveAs(fullpath);
                obj.product_img1 = filename;
            }
            e.products.Add(obj);
            e.SaveChanges();
            return RedirectToAction("ManageProducts");
        }
    }
}