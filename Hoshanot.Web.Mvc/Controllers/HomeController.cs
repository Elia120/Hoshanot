using Hoshanot.Web.DataAccess.DataSource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hoshanot.Web.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private HoshanotContext db = new HoshanotContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Contact()
        {

            return View();
        }
        public ActionResult Products()
        {
            ViewBag.Message = "Produkte";

            return View(db.Products);
        }
        [HttpPost]
        public ActionResult ProductsCookies(int index, int value)
        {
            string indexName = "product" + index;
            if (Request.Cookies.AllKeys.Contains(indexName))
            {
                HttpCookie cookie = Request.Cookies[indexName];
                cookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(cookie);
            }
            HttpCookie indexProducts = new HttpCookie(indexName, value.ToString())
            {
                Expires = DateTime.Now.AddDays(1)
            };
            Response.Cookies.Add(indexProducts);
            return RedirectToAction("Products");
            
        }

        public ActionResult RemoveProductsCookies(int? index)
        {
            if (index==null)
            {
                return RedirectToAction("ShoppingCart");
            }
            string indexName = "product" + index;
            if (Request.Cookies.AllKeys.Contains(indexName))
            {
                HttpCookie cookie = Request.Cookies[indexName];
                cookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(cookie);
            }
            return RedirectToAction("ShoppingCart");

        }
        public ActionResult ShoppingCart()
        {
            List<DataAccess.Models.Product> products = new List<DataAccess.Models.Product>();
            var x = db.Products.Count();
            for (int i = 0; i < x; i++)
            {
                string indexName = "product" + i;
                if (Request.Cookies.AllKeys.Contains(indexName))
                {
                    HttpCookie cookie = Request.Cookies[indexName];
                    var temprod = (from u in db.Products where i+1 == u.ProductID select u).FirstOrDefault();
                    temprod.Amount =int.Parse( cookie.Value);
                    products.Add(temprod);
                }
            }
            return View(products);
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