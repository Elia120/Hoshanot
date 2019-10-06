using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using Hoshanot.Web.DataAccess.DataSource;
using Hoshanot.Web.DataAccess.Models;

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
        public ActionResult ShoppingCart()
        {
            return View();
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

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
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
