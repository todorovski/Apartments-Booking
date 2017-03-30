using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVC_MyHotel_example.Models;

namespace MVC_MyHotel_example.Controllers
{
    //[Authorize(Roles = "Administrator")]
    public class ReservationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Reservations
        public ActionResult Index()
        {
            if (User.IsInRole("Administrator"))
            {
                var reservations = db.Reservations.Include(r => r.Apartment);
                return View(reservations.ToList());
            }
            else
                return RedirectToAction("NoAccess", "Home");
        }

        public ActionResult MyReservations()
        {
            if (Request.IsAuthenticated && !User.IsInRole("Administrator"))
            {
                string CustomerUsername = User.Identity.Name;
                var reservations = db.Reservations.Include(r => r.Apartment);
                return View(reservations.Where(r => r.CustomerUsername == CustomerUsername).ToList());
            }
            else
                return RedirectToAction("NoAccess", "Home");
        }

        // GET: Reservations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            return View(reservation);
        }

        // GET: Reservations/Create
        //public ActionResult Create()
        //{
        //    ViewBag.ApartmentId = new SelectList(db.Apartments, "ID", "description");
        //    return View();
        //}

        public ActionResult Create(int ApartmentId, string CustomerUsername)
        {
            if (Request.IsAuthenticated && !User.IsInRole("Administrator"))
            {
                ViewData["ApartmentId"] = ApartmentId;
                ViewData["CustomerUsername"] = CustomerUsername;
                ViewBag.ApartmentId = new SelectList(db.Apartments, "ID", "description");
                return View();
            }
            else
                return RedirectToAction("NoAccess", "Home");
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CustomerUsername,CustomerName,CustomerSurname,From,To,ApartmentId")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                db.Apartments.Where(a => a.ID == reservation.ApartmentId).FirstOrDefault().booked = true;
                db.Reservations.Add(reservation);
                db.SaveChanges();
                return RedirectToAction("MyReservations");
            }

            ViewBag.ApartmentId = new SelectList(db.Apartments, "ID", "description", reservation.ApartmentId);
            return View(reservation);
        }

        // GET: Reservations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Request.IsAuthenticated && !User.IsInRole("Administrator"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Reservation reservation = db.Reservations.Find(id);
                if (reservation == null)
                {
                    return HttpNotFound();
                }
                ViewBag.ApartmentId = new SelectList(db.Apartments, "ID", "description", reservation.ApartmentId);
                return View(reservation);
            }
            else
                return RedirectToAction("NoAccess", "Home");
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CustomerUsername,CustomerName,CustomerSurname,From,To,ApartmentId")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reservation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("MyReservations");
            }
            ViewBag.ApartmentId = new SelectList(db.Apartments, "ID", "description", reservation.ApartmentId);
            return View(reservation);
        }

        // GET: Reservations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Request.IsAuthenticated && !User.IsInRole("Administrator"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Reservation reservation = db.Reservations.Find(id);
                if (reservation == null)
                {
                    return HttpNotFound();
                }
                db.Apartments.Where(a => a.ID == reservation.ApartmentId).FirstOrDefault().booked = false;
                return View(reservation);
            }
            else
                return RedirectToAction("NoAccess", "Home");
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Reservation reservation = db.Reservations.Find(id);
            db.Reservations.Remove(reservation);
            db.SaveChanges();
            return RedirectToAction("MyReservations");
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
