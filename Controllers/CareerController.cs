using SensenbrennerHospital.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace SensenbrennerHospital.Controllers
{
    public class CareerController : Controller
    {
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client;
        static CareerController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false
            };
            client = new HttpClient(handler);
            client.BaseAddress = new Uri("https://localhost:44336/api/");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        // GET: Career
        public ActionResult Index()
        {
            string url = "CareerData/ListCareers";
            HttpResponseMessage httpResponse = client.GetAsync(url).Result;

            if (httpResponse.IsSuccessStatusCode)
            {
                IEnumerable<CareerDto> CareerList = httpResponse.Content.ReadAsAsync<IEnumerable<CareerDto>>().Result;
                ViewBag.isadmin = User.IsInRole("Admin");
                return View(CareerList);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Career/Details/5
        public ActionResult Details(int id)
        {
            string url = "CareerData/GetCareer/" + id;
            HttpResponseMessage httpResponse = client.GetAsync(url).Result;

            if (httpResponse.IsSuccessStatusCode)
            {
                CareerDto selectedCareer = new CareerDto();
                selectedCareer = httpResponse.Content.ReadAsAsync<CareerDto>().Result;
                selectedCareer.isadmin = User.IsInRole("Admin");
                return View(selectedCareer);

            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Career/Create
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Career/Create
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(Career NewCareer)
        {
            string url = "CareerData/AddCareer";

            HttpContent content = new StringContent(jss.Serialize(NewCareer));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage httpResponse = client.PostAsync(url, content).Result;
            Debug.WriteLine(jss.Serialize(NewCareer));

            if (httpResponse.IsSuccessStatusCode)
            {
                int CareerID = httpResponse.Content.ReadAsAsync<int>().Result;

                return RedirectToAction("Details", new { id = CareerID });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Career/Edit/5
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            string url = "CareerData/GetCareer/" + id;
            HttpResponseMessage httpResponse = client.GetAsync(url).Result;

            if (httpResponse.IsSuccessStatusCode)
            {
                CareerDto selectedCareer = new CareerDto();
                selectedCareer = httpResponse.Content.ReadAsAsync<CareerDto>().Result;

                return View(selectedCareer);

            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: Career/Edit/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id, Career selectedCareer)
        {
            string url = "CareerData/UpdateCareer/" + id;
            HttpContent content = new StringContent(jss.Serialize(selectedCareer));
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

        // GET: Career/DeleteConfirm/5
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "CareerData/GetCareer/" + id;
            HttpResponseMessage httpResponse = client.GetAsync(url).Result;

            if (httpResponse.IsSuccessStatusCode)
            {
                CareerDto selectedCareer = new CareerDto();
                selectedCareer = httpResponse.Content.ReadAsAsync<CareerDto>().Result;

                return View(selectedCareer);

            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: Career/Delete/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            string url = "CareerData/DeleteCareer/" + id;
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
