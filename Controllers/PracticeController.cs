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
    public class PracticeController : Controller
    {
        private JavaScriptSerializer JSS = new JavaScriptSerializer();
        private static readonly HttpClient Client;

        static PracticeController()
        {
            HttpClientHandler Handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false
            };
            Client = new HttpClient(Handler);
            Client.BaseAddress = new Uri("https://localhost:44336/api/");
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        // GET: Practices
        public ActionResult ListPractices()
        {
            string URL = "PracticeData/GetPractices";
            HttpResponseMessage HttpResponse = Client.GetAsync(URL).Result;

            if (HttpResponse.IsSuccessStatusCode)
            {
                IEnumerable<PracticeDTO> PracticeList = HttpResponse.Content.ReadAsAsync<IEnumerable<PracticeDTO>>().Result;

                return View(PracticeList);
            } else
            {
                return RedirectToAction("List");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(Practice NewPractice)
        {
            string URL = "PracticeData/AddPractice";

            HttpContent Content = new StringContent(JSS.Serialize(NewPractice));
            Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage HttpResponse = Client.PostAsync(URL, Content).Result;

            if (HttpResponse.IsSuccessStatusCode)
            {
                int PracticeID = HttpResponse.Content.ReadAsAsync<int>().Result;
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            CreatePractice ViewModel = new CreatePractice();

            string URL = "DepartmentData/ListDepartments";
            HttpResponseMessage Response = Client.GetAsync(URL).Result;
            IEnumerable<DepartmentDto> ListOfDepartments = Response.Content.ReadAsAsync<IEnumerable<DepartmentDto>>().Result;
            ViewModel.AllDepartments = ListOfDepartments;

            return View(ViewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Update(int id)
        {
            string URL = "PracticeData/FindPractice/" + id;

            HttpResponseMessage Response = Client.GetAsync(URL).Result;

            if (Response.IsSuccessStatusCode)
            {
                PracticeDTO SelectedPractice = Response.Content.ReadAsAsync<PracticeDTO>().Result;

                return View(SelectedPractice);
            }
            else
            {
                return RedirectToAction("Error");
            }    
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Update(int id, Practice PracticeInfo)
        {
            string URL = "PracticeData/UpdatePractice/" + id;

            HttpContent Content = new StringContent(JSS.Serialize(PracticeInfo));

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


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            string URL = "PracticeData/DeletePractice/" + id;
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

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirm(int id)
        {
            string URL = "PracticeData/GetPractices/" + id;
            HttpResponseMessage HttpResponse = Client.GetAsync(URL).Result;

            if (HttpResponse.IsSuccessStatusCode)
            {
                PracticeDTO Practice = new PracticeDTO();
                Practice = HttpResponse.Content.ReadAsAsync<PracticeDTO>().Result;

                return View(Practice);
            } else
            {
                return RedirectToAction("Error");
            }
        }

        [HttpGet]
        public IEnumerable<PracticeDTO> ShowPractice()
        {
            string URL = "PracticeData/GetListOfPractices";
            HttpResponseMessage HttpResponse = Client.GetAsync(URL).Result;

            if (HttpResponse.IsSuccessStatusCode)
            {
                IEnumerable<PracticeDTO> PracticeList = HttpResponse.Content.ReadAsAsync<IEnumerable<PracticeDTO>>().Result;

                return PracticeList;
            }
            else
            {
                return null;
            }
        }

        

    }
}