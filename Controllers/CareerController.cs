using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SensenbrennerHospital.Controllers
{
    public class CareerController : Controller
    {
        // GET: Career
        public ActionResult Index()
        {
            return View();
        }

        // GET: Career/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Career/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Career/Create
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

        // GET: Career/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Career/Edit/5
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

        // GET: Career/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Career/Delete/5
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
