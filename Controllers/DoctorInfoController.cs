using SensenbrennerHospital.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace SensenbrennerHospital.Controllers
{
    public class DoctorInfoController : Controller
    {
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client;

        static DoctorInfoController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false
            };
            client = new HttpClient(handler);
            client.BaseAddress = new Uri("https://localhost:44336/api/");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public ActionResult ListDoctors()
        {
            string URL = "DoctorData/AddDoctor";
            HttpResponseMessage httpResponse = client.GetAsync(URL).Result;
            
            if (httpResponse.IsSuccessStatusCode)
            {
                IEnumerable<DoctorDTO> DoctorsList = httpResponse.Content.ReadAsAsync<IEnumerable<DoctorDTO>>().Result;
                return View(DoctorsList);
            } else
            {
                return RedirectToAction("Error");
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

    }
}