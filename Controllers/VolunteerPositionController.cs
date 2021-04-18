using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using SensenbrennerHospital.Models;
using SensenbrennerHospital.Models.ViewModels;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Diagnostics;
using System.Web.Script.Serialization;


namespace SensenbrennerHospital.Controllers
{
    public class VolunteerPositionController : Controller
    {



        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client;


        static VolunteerPositionController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false
            };
            client = new HttpClient(handler);

            client.BaseAddress = new Uri("https://localhost:44336/api/");
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));




        }



        // GET: VolunteerPosition/List
        public ActionResult List()
        {
            string url = "volunteerpositiondata/getvolunteerpositions";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<VolunteerPositionDto> SelectedVolunteerPositions = response.Content.ReadAsAsync<IEnumerable<VolunteerPositionDto>>().Result;
                Debug.WriteLine("I have grabbed " + SelectedVolunteerPositions.Count());
                return View(SelectedVolunteerPositions);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: VolunteerPosition/Details/5
        public ActionResult Details(int id)
        {
            ShowVolunteerPosition ViewModel = new ShowVolunteerPosition();
            string url = "volunteerpositiondata/findvolunteerposition/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Catch the status code (200 OK, 301 REDIRECT), etc.

            if (response.IsSuccessStatusCode)
            {
                //VolunteerPosition goes in Data Transfer Object
                VolunteerPositionDto SelectedVolunteerPosition = response.Content.ReadAsAsync<VolunteerPositionDto>().Result;
                ViewModel.volunteerposition = SelectedVolunteerPosition;


                url = "volunteerpositiondata/finddepartmentforvolunteerposition/" + id;
                response = client.GetAsync(url).Result;
                DepartmentDto SelectedDepartment = response.Content.ReadAsAsync<DepartmentDto>().Result;
                ViewModel.department = SelectedDepartment;

                return View(ViewModel);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: VolunteerPosition/Create
        public ActionResult Create()
        {
            UpdateVolunteerPosition ViewModel = new UpdateVolunteerPosition();
            //get information about departments that may also need this volunteer position
            string url = "departmentdata/getdepartments";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<DepartmentDto> PotentialDepartments = response.Content.ReadAsAsync<IEnumerable<DepartmentDto>>().Result;
            ViewModel.alldepartments = PotentialDepartments;

            return View(ViewModel);
        }

        // POST: VolunteerPosition/Create
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Create(VolunteerPosition VolunteerPositionInfo)
        {
            Debug.WriteLine(VolunteerPositionInfo.Name);
            string url = "volunteerpositiondata/addvolunteerposition";
            Debug.WriteLine(jss.Serialize(VolunteerPositionInfo));
            HttpContent content = new StringContent(jss.Serialize(VolunteerPositionInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {

                int volunteerpositionid = response.Content.ReadAsAsync<int>().Result;
                return RedirectToAction("Details", new { id = volunteerpositionid });
            }
            else
            {
                return RedirectToAction("Error");
            }


        }

        // GET: VolunteerPosition/Edit/8
        public ActionResult Edit(int id)
        {
            UpdateVolunteerPosition ViewModel = new UpdateVolunteerPosition();

            string url = "volunteerpositiondata/findvolunteerposition/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;


            if (response.IsSuccessStatusCode)
            {
                //Put data into volunteerposition data transfer object
                VolunteerPositionDto SelectedVolunteerPosition = response.Content.ReadAsAsync<VolunteerPositionDto>().Result;
                ViewModel.volunteerposition = SelectedVolunteerPosition;

                //get information about departments that may also need this volunteer position
                url = "departmentdata/getdepartments";
                response = client.GetAsync(url).Result;
                IEnumerable<DepartmentDto> PotentialDepartments = response.Content.ReadAsAsync<IEnumerable<DepartmentDto>>().Result;
                ViewModel.alldepartments = PotentialDepartments;

                return View(ViewModel);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: VolunteerPosition/Delete/5
        [HttpGet]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "volunteerpositiondata/findvolunteerposition/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                //Put data into volunteerposition data transfer object
                VolunteerPositionDto SelectedVolunteerPosition = response.Content.ReadAsAsync<VolunteerPositionDto>().Result;
                return View(SelectedVolunteerPosition);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: VolunteerPosition/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Delete(int id)
        {
            string url = "volunteerpositiondata/deletevolunteerposition/" + id;

            HttpContent content = new StringContent("");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {

                return RedirectToAction("List");
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