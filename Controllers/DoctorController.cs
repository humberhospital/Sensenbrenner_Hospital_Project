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
    public class DoctorController : Controller
    {
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient Client;

        static DoctorController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false
            };
            Client = new HttpClient(handler);
            Client.BaseAddress = new Uri("http://sensenbrennerhospital-env.eba-ev233pkq.us-east-2.elasticbeanstalk.com/api/");
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }


        public ActionResult ListDoctors()
        {
            string URL = "DoctorData/GetDoctors";
            HttpResponseMessage httpResponse = Client.GetAsync(URL).Result;

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

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(Doctor NewDoctor)
        {
            string URL = "DoctorData/AddDoctor";

            HttpContent Content = new StringContent(jss.Serialize(NewDoctor));
            Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage HttpResponse = Client.PostAsync(URL, Content).Result;

            if (HttpResponse.IsSuccessStatusCode)
            {
                int DoctorID = HttpResponse.Content.ReadAsAsync<int>().Result;
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Update(int id)
        {
            string URL = "DoctorData/FindDoctor/" + id;

            HttpResponseMessage Response = Client.GetAsync(URL).Result;

            if (Response.IsSuccessStatusCode)
            {
                DoctorDTO SelectedDoctor = Response.Content.ReadAsAsync<DoctorDTO>().Result;
                return View(SelectedDoctor);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Update(int id, Doctor DoctorInfo)
        {
            string URL = "DoctorData/UpdateDoctor/" + id;

            HttpContent Content = new StringContent(jss.Serialize(DoctorInfo));
            Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage Response = Client.PostAsync(URL, Content).Result;

            if (Response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirm(int id)
        {
            string URL = "DoctorData/GetDoctors/" + id;
            HttpResponseMessage HttpResponse = Client.GetAsync(URL).Result;

            if (HttpResponse.IsSuccessStatusCode)
            {
                DoctorDTO Doctor = new DoctorDTO();
                Doctor = HttpResponse.Content.ReadAsAsync<DoctorDTO>().Result;

                return View(Doctor);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            string URL = "DoctorData/DeleteDoctor/" + id;
            HttpContent Content = new StringContent("");
            HttpResponseMessage HttpResponse = Client.PostAsync(URL, Content).Result;

            if (HttpResponse.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }    
        }

        //public ActionResult Update(int id)
        //{
        //    string URL = "DoctorData/"
        //}

        [HttpGet]
        public IEnumerable<DoctorDTO> ShowDoctor()
        {
            string URL = "DoctorData/GetListOfDoctors";
            HttpResponseMessage HttpResponse = Client.GetAsync(URL).Result;

            if (HttpResponse.IsSuccessStatusCode)
            {
                IEnumerable<DoctorDTO> DoctorList = HttpResponse.Content.ReadAsAsync<IEnumerable<DoctorDTO>>().Result;

                return DoctorList;
            }
            else
            {
                return null;
            }    
        }

    }
}