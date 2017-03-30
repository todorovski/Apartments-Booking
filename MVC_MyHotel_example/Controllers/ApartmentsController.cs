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
    public class ApartmentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Apartments
        public ActionResult Index()
        {
            return View(db.Apartments.ToList());
        }

        // GET: Apartments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Apartment apartment = db.Apartments.Find(id);
            if (apartment == null)
            {
                return HttpNotFound();
            }

            return View(apartment);
        }

        // GET: Apartments/Create
        //public ActionResult Create()
        //{
        //    //ViewData["pom"] = "haha1";
        //    return View();
        //}

        //public ActionResult Create(string hotelID)
        //{
        //    ViewData["pom"] = "haha";
        //    ViewData["hotelID"] = hotelID;
        //    return View();
        //}

        // good
        public ActionResult Create(int? hotelID)
        {
            if (User.IsInRole("Administrator"))
            {
                //ViewData["pom"] = "final";
                ViewData["hotelID"] = hotelID;
                return View();
            }
            else
                return RedirectToAction("NoAccess", "Home");
        }

        //public ActionResult AddApartment(int? hotelID)
        //{
        //    //ViewData["pom"] = "final";
        //    ViewData["hotelID"] = hotelID;
        //    return View("Create");
        //}

        //public ActionResult Create2(int? hotelID)
        //{
        //    ViewData["pom"] = "haha2";
        //    //ViewData["hotelID"] = 1;
        //    ViewData["hotelID"] = hotelID;
        //    return View("Create");
        //}

        // POST: Apartments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,floor,rooms,description,rating,image,hotelID")] Apartment apartment, HttpPostedFileBase file)
        {
            //ViewData["file"] = file.FileName.ToString();
            //return View("Test");
            //return View("haha" + file.ToString());

            //ViewData["pom"] = "haha2";
            if (ModelState.IsValid)
            {
                var fileName = Path.GetFileName(file.FileName); //getting only file name(ex-ganesh.jpg)  
                var ext = Path.GetExtension(file.FileName); //getting the extension(ex-.jpg)  

                string name = Path.GetFileNameWithoutExtension(fileName); //getting file name without extension  
                string myfile = name + "_" + apartment.ID + ext; //appending the name with id   
                var path = Path.Combine(Server.MapPath("~/Content/ApartmentsImg"), myfile);  // store the file inside ~/project folder(Img) 

                apartment.image = myfile;
                db.Apartments.Add(apartment);
                db.SaveChanges();

                file.SaveAs(path);
                //return RedirectToAction("Index");
                return RedirectToAction("Apartments", "Hotels", new { id = apartment.hotelID });
            }

            return View(apartment);
            
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(int id, [Bind(Include = "ID,floor,rooms,description,rating,image,hotelID")] Apartment apartment)
        //{
        //    ViewData["hotelID"] = id;
        //    ViewData["pom"] = "haha3";
        //    if (ModelState.IsValid)
        //    {
        //        db.Apartments.Add(apartment);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(apartment);
        //}

        // GET: Apartments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (User.IsInRole("Administrator"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Apartment apartment = db.Apartments.Find(id);
                if (apartment == null)
                {
                    return HttpNotFound();
                }
                return View(apartment);
            }
            else
                return RedirectToAction("NoAccess", "Home");
        }

        // POST: Apartments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,floor,rooms,description,rating,image,hotelID")] Apartment apartment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(apartment).State = EntityState.Modified;
                db.SaveChanges();
                //return RedirectToAction("Index");
                return RedirectToAction("Apartments", "Hotels", new { id = apartment.hotelID });

            }
            return View(apartment);
        }

        // GET: Apartments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (User.IsInRole("Administrator"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Apartment apartment = db.Apartments.Find(id);
                if (apartment == null)
                {
                    return HttpNotFound();
                }
                return View(apartment);
            }
            else
                return RedirectToAction("NoAccess", "Home");
        }

        // POST: Apartments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Apartment apartment = db.Apartments.Find(id);
            db.Apartments.Remove(apartment);
            db.SaveChanges();
            //return RedirectToAction("Index");
            return RedirectToAction("Apartments", "Hotels", new { id = apartment.hotelID });
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
