using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using SensenbrennerHospital.Models;

namespace SensenbrennerHospital.Controllers
{
    public class AppointmentBookingController : Controller
    {
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client;
        static AppointmentBookingController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false
            };
            client = new HttpClient(handler);
            client.BaseAddress = new Uri("https://localhost:44375/api/");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        // Homepage for this table shows all the bookings, this can be adjusted later
        // GET: AppointmentBooking
        public ActionResult Index()
        {
            string url = "AppointmentBookingData/ListAppointmentBookings";
            HttpResponseMessage httpResponse = client.GetAsync(url).Result;

            if (httpResponse.IsSuccessStatusCode)
            {
                IEnumerable<AppointmentBooking> BookingList = httpResponse.Content.ReadAsAsync<IEnumerable<AppointmentBooking>>().Result;
                return View(BookingList);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // Shows detail for a certain entry
        // GET: AppointmentBooking/Details/5
        public ActionResult Details(int id)
        {
            string url = "AppointmentBookingData/GetAppointmentBooking/" + id;
            HttpResponseMessage httpResponse = client.GetAsync(url).Result;

            if (httpResponse.IsSuccessStatusCode)
            {
                AppointmentBooking selectedBooking = new AppointmentBooking();
                selectedBooking = httpResponse.Content.ReadAsAsync<AppointmentBooking>().Result;

                return View(selectedBooking);

            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: AppointmentBooking/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AppointmentBooking/Create
        [HttpPost]
        public ActionResult Create(AppointmentBooking NewAppointmentBooking)
        {
            string url = "AppointmentBookingData/AddAppointmentBooking";

            HttpContent content = new StringContent(jss.Serialize(NewAppointmentBooking));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage httpResponse = client.PostAsync(url, content).Result;
            Debug.WriteLine(jss.Serialize(NewAppointmentBooking));

            if (httpResponse.IsSuccessStatusCode)
            {
                int BookingID = httpResponse.Content.ReadAsAsync<int>().Result;

                return RedirectToAction("Details", new { id = BookingID });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: AppointmentBooking/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "AppointmentBookingData/GetAppointmentBooking/" + id;
            HttpResponseMessage httpResponse = client.GetAsync(url).Result;

            if (httpResponse.IsSuccessStatusCode)
            {
                AppointmentBooking selectedBooking = new AppointmentBooking();
                selectedBooking = httpResponse.Content.ReadAsAsync<AppointmentBooking>().Result;

                return View(selectedBooking);

            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: AppointmentBooking/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, AppointmentBooking selectedBooking)
        {
            string url = "AppointmentBookingData/UpdateAppointmentBooking/" + id;
            HttpContent content = new StringContent(jss.Serialize(selectedBooking));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage httpResponse = client.PostAsync(url, content).Result;

            if (httpResponse.IsSuccessStatusCode)
            {
                return RedirectToAction("Details", new { id = id });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: AppointmentBooking/Delete/5
        [HttpGet]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "AppointmentBookingData/GetAppointmentBooking/" + id;
            HttpResponseMessage httpResponse = client.GetAsync(url).Result;

            if (httpResponse.IsSuccessStatusCode)
            {
                AppointmentBooking selectedBooking = new AppointmentBooking();
                selectedBooking = httpResponse.Content.ReadAsAsync<AppointmentBooking>().Result;

                return View(selectedBooking);

            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: AppointmentBooking/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "AppointmentBookingData/DeleteAppointmentBooking/" + id;
            HttpContent content = new StringContent("");
            HttpResponseMessage httpResponse = client.PostAsync(url, content).Result;

            if (httpResponse.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
    }
}
