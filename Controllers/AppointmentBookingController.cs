using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SensenbrennerHospital.Controllers
{
    public class AppointmentBookingController : Controller
    {
        // GET: AppointmentBooking
        public ActionResult Index()
        {
            return View();
        }

        // GET: AppointmentBooking/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AppointmentBooking/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AppointmentBooking/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: AppointmentBooking/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AppointmentBooking/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: AppointmentBooking/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AppointmentBooking/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
