using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using Hoshanot.Web.DataAccess.DataSource;
using Hoshanot.Web.DataAccess.Models;
using Hoshanot.Web.Mvc.ViewModels;

namespace Hoshanot.Web.Mvc.Controllers
{
    public class UsersController : Controller
    {
        private HoshanotContext db = new HoshanotContext();

        // GET: Users
        public ActionResult Index()
        {
            return RedirectToAction("Login");
        }
        // GET: Users
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login([Bind(Include = "EMail,Password")] User user)
        {
            User DbUser = (from u in db.Users where u.EMail == user.EMail && u.Password == user.Password select u).FirstOrDefault();
            if (DbUser != null)
            {
                CreateCookie(DbUser);
                RedirectToAction("Index");

            }
            else
            {
                ModelState.AddModelError(string.Empty, "E-Mail oder Passwort ist inkorrekt");
                return View("Login");
            }

            return View();
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "EMail,Password,TelNr")] User user)
        {
            if (ModelState.IsValid)
            {
                var User = new User
                {
                    UserID = user.UserID,
                    EMail = user.EMail,
                    Password = user.Password,
                    TelNr = user.TelNr
                };

                db.Users.Add(User);
                db.SaveChanges();
                CreateCookie(User);
                return RedirectToAction("Account");
            }
            return View(user);
        }
        public ActionResult Logout()
        {
            if (Request.Cookies.AllKeys.Contains("sessionID"))
            {
                HttpCookie cookie = Request.Cookies["sessionID"];
                cookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(cookie);
            }
            return RedirectToAction("Login");
        }
        public ActionResult Account()
        {
            User DbUser = CheckInDbAndReturnUser();
            if (DbUser!=null)
            {
                return View(DbUser);
            }
            else
            {
                RedirectToAction("Logout");
            }
            return View();

        }
        public ActionResult Checkout()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Checkout(Checkout user)
        {
            TempData["user"] = user;
            return RedirectToAction("Confirm");

        }
        public ActionResult Confirm()
        {
            List<Product> products = new List<DataAccess.Models.Product>();
            var x = db.Products.Count();
            for (int i = 0; i < x; i++)
            {
                string indexName = "product" + i;
                if (Request.Cookies.AllKeys.Contains(indexName))
                {
                    HttpCookie cookie = Request.Cookies[indexName];
                    var temprod = (from u in db.Products where i + 1 == u.ProductID select u).FirstOrDefault();
                    temprod.Amount = int.Parse(cookie.Value);
                    products.Add(temprod);
                }
            }
            Checkout User1 = (Checkout)TempData["user"];
            
            if (User1==null)
            {
                return RedirectToAction("ShoppingCart", "Home");
            }

            var User = new User
            {
                EMail = User1.Email,
                Password = null,
                TelNr =User1.TelNr,
            };

            db.Users.Add(User);
            db.SaveChanges();
            var Order = new Order
            {
                OrderDate = DateTime.Now,
                Paid = false,
                UserID = User.UserID,
                User = User
            };
            db.Orders.Add(Order);
            db.SaveChanges();
            User1.OrderID = Order.OrderID;

            decimal TotalAmount = 0;
            for (int i = 0; i < products.Count; i++)
            {
                var OrderDeatail = new OrderDetail
                {
                    OrderID = Order.OrderID,
                    Order = Order,
                    ProductID = products[i].ProductID,
                    Amount = products[i].Amount
                };
                db.OrderDetails.Add(OrderDeatail);
                TotalAmount += products[i].Amount * products[i].Price;
            }
            Order.Total = TotalAmount;
            var Adress = new Address
            {
                DeliverTyp = (DataAccess.Models.DeliverTyp)User1.DeliverTyp,
                Street = User1.Strasse,
                HouseNr = User1.Hausnummer.ToString(),
                FirstName = User1.FistName,
                LastName = User1.LastName,
                UserID = User.UserID,
                User = User
            };
            db.Addresses.Add(Adress);
            db.SaveChanges();
            ViewBag.Data = User1;

            return View(products);

        }
        public ActionResult Confirmed(int id)
        {
            ViewBag.Data = id;
            Order temp = (from o in db.Orders where id == o.OrderID select o).FirstOrDefault();
            temp.Confirmed = true;
            db.SaveChanges();
            SendMail(temp.User.EMail);
            var x = db.Products.Count();
            for (int i = 0; i < x; i++)
            {
                string indexName = "product" + i;
                if (Request.Cookies.AllKeys.Contains(indexName))
                {
                    HttpCookie cookie = Request.Cookies[indexName];
                    cookie.Expires=DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(cookie);
                }
            }
            return View();
        }

        private void SendMail(string eMail)
        {
            var fromAddress = new MailAddress("hoshanotschweiz@gmail.com", "HOSHANOT.CH");
            var toAddress = new MailAddress(eMail);


            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, "3412534125Es")
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = "Bestellung",
                Body = "Vielen Dank für ihre Bestellung."
            })
            {
                smtp.Send(message);
            }
            smtp.Dispose();
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // Aktivieren Sie zum Schutz vor übermäßigem Senden von Angriffen die spezifischen Eigenschaften, mit denen eine Bindung erfolgen soll. Weitere Informationen 
        // finden Sie unter https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserID,EMail,Password,TelNr,SessionID,IsAdmin")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

         void CreateCookie(User DbUser)
        {
            if (Request.Cookies.AllKeys.Contains("sessionID"))
            {
                HttpCookie cookie = Request.Cookies["sessionID"];
                cookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(cookie);
            }
            var bytes = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(bytes);
                string hash = BitConverter.ToString(bytes);
                HttpCookie SessionID = new HttpCookie("sessionID", hash);
                DbUser.SessionID = hash;
                db.Entry(DbUser).State = EntityState.Modified;
                db.SaveChanges();
                Response.Cookies.Add(SessionID);
            }
        }
        User CheckInDbAndReturnUser()
        {
            if (Request.Cookies.AllKeys.Contains("sessionID"))
            {
                HttpCookie cookie = Request.Cookies["sessionID"];
                User DbUser = (from u in db.Users where u.SessionID == cookie.Value select u).FirstOrDefault();
                CreateCookie(DbUser);
                return DbUser;
            }
            else
            {
                return null;
            }
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
