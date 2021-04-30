using SensenbrennerHospital.Models;
using SensenbrennerHospital.Models.ViewModels;
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
            List<ListDoctor> ViewModel = new List<ListDoctor>();
            HttpResponseMessage httpResponse = Client.GetAsync(URL).Result;

            if (httpResponse.IsSuccessStatusCode)
            {
                IEnumerable<DoctorDTO> DoctorsList = httpResponse.Content.ReadAsAsync<IEnumerable<DoctorDTO>>().Result;

                foreach (var item in DoctorsList)
                {
                    URL = "PracticeData/FindPractice/" + item.PracticeID;
                    httpResponse = Client.GetAsync(URL).Result;
                    PracticeDTO Practice = httpResponse.Content.ReadAsAsync<PracticeDTO>().Result;
                    ListDoctor NewList = new ListDoctor
                    {
                        Doctor = item,
                        Practice = Practice
                    };
                    ViewModel.Add(NewList);
                }

                return View(ViewModel);
            } else
            {
                return RedirectToAction("Error");
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            CreateDoctor ViewModel = new CreateDoctor();
            string URL = "PracticeData/GetListOfPractices";
            HttpResponseMessage HttpResponse = Client.GetAsync(URL).Result;

            if (HttpResponse.IsSuccessStatusCode)
            {
                List<PracticeDTO> Practices = HttpResponse.Content.ReadAsAsync<List<PracticeDTO>>().Result;
                ViewModel.Practice = Practices;
                return View(ViewModel);
            }
            else
            {
                return RedirectToAction("Error");
            }
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
                return RedirectToAction("ListDoctors");
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
            CreateDoctor ViewModel = new CreateDoctor();
            string URL = "DoctorData/FindDoctor/" + id;

            HttpResponseMessage Response = Client.GetAsync(URL).Result;

            if (Response.IsSuccessStatusCode)
            {
                DoctorDTO SelectedDoctor = Response.Content.ReadAsAsync<DoctorDTO>().Result;
                ViewModel.Doctor = SelectedDoctor;
                URL = "PracticeData/GetListOfPractices";
                Response = Client.GetAsync(URL).Result;
                List<PracticeDTO> PracticeList = Response.Content.ReadAsAsync<List<PracticeDTO>>().Result;
                ViewModel.Practice = PracticeList;
                return View(ViewModel);
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
            return RedirectToAction("ListDoctors");
            //if (Response.IsSuccessStatusCode)
            //{
            //    return RedirectToAction("ListDoctors");
            //}
            //else
            //{
            //    return RedirectToAction("Error");
            //}
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirm(int id)
        {
            string URL = "DoctorData/GetDoctor/" + id;
            ListDoctor ViewModel = new ListDoctor();


            HttpResponseMessage HttpResponse = Client.GetAsync(URL).Result;

            if (HttpResponse.IsSuccessStatusCode)
            {
                DoctorDTO Doctor = HttpResponse.Content.ReadAsAsync<DoctorDTO>().Result;
                ViewModel.Doctor = Doctor;

                URL = "PracticeData/FindPractice/" + Doctor.PracticeID;
                HttpResponse = Client.GetAsync(URL).Result;

                PracticeDTO Practice = HttpResponse.Content.ReadAsAsync<PracticeDTO>().Result;
                ViewModel.Practice = Practice;

                return View(ViewModel);
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
                return RedirectToAction("ListDoctors");
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
        public ActionResult DoctorDetails(int id)
        {
            string URL = "DoctorData/GetDoctor/" + id;
            HttpResponseMessage HttpResponse = Client.GetAsync(URL).Result;

            if (HttpResponse.IsSuccessStatusCode)
            {
                DoctorDTO DoctorList = HttpResponse.Content.ReadAsAsync<DoctorDTO>().Result;

                return View(DoctorList);
            }
            else
            {
                return RedirectToAction("Error");
            }    
        }

        public ActionResult Error()
        {
            return View();
        }

    }
}