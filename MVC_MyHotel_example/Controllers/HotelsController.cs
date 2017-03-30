using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVC_MyHotel_example.Models;
using System.IO;

namespace MVC_MyHotel_example.Controllers
{
    public class HotelsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Hotels
        public ActionResult Index()
        {
            return View(db.Hotels.ToList());
        }

        public ActionResult Apartments(int? id)
        {
            ViewData["hotelID"] = id;
            ViewData["Hotel"] = db.Hotels.Where(h => h.ID == id).FirstOrDefault();
            return View(db.Apartments.Where(a => a.hotelID == id).ToList());
        }

        // GET: Hotels/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Hotel hotel = db.Hotels.Find(id);
            if (hotel == null)
            {
                return HttpNotFound();
            }
            return View(hotel);
        }

        // GET: Hotels/Create
        public ActionResult Create()
        {
            if (User.IsInRole("Administrator"))
            {
                return View();
            }
            else
                return RedirectToAction("NoAccess", "Home");
        }

        // POST: Hotels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "ID,name,address,city,stars")] Hotel hotel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Hotels.Add(hotel);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(hotel);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,name,address,city,image,stars")] Hotel hotel, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                var fileName = Path.GetFileName(file.FileName);
                var ext = Path.GetExtension(file.FileName);

                string name = Path.GetFileNameWithoutExtension(fileName);
                string myfile = name + "_" + hotel.ID + ext;
                var path = Path.Combine(Server.MapPath("~/Content/HotelsImg"), myfile);

                hotel.image = myfile;
                db.Hotels.Add(hotel);
                db.SaveChanges();

                file.SaveAs(path);

                return RedirectToAction("Index");
            }
            return View(hotel);
        }

        // GET: Hotels/Edit/5
        public ActionResult Edit(int? id)
        {
            if (User.IsInRole("Administrator"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Hotel hotel = db.Hotels.Find(id);
                if (hotel == null)
                {
                    return HttpNotFound();
                }
                return View(hotel);
            }
            else
                return RedirectToAction("NoAccess", "Home");
        }

        // POST: Hotels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,name,address,city,stars,image")] Hotel hotel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hotel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(hotel);
        }

        // GET: Hotels/Delete/5
        public ActionResult Delete(int? id)
        {
            if (User.IsInRole("Administrator"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Hotel hotel = db.Hotels.Find(id);
                if (hotel == null)
                {
                    return HttpNotFound();
                }
                return View(hotel);
            }
            else
                return RedirectToAction("NoAccess", "Home");
        }

        // POST: Hotels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Hotel hotel = db.Hotels.Find(id);
            db.Hotels.Remove(hotel);
            db.SaveChanges();
            return RedirectToAction("Index");
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
