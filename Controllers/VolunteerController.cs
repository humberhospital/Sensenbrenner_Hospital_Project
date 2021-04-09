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
    public class VolunteerController : Controller
    {



        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client;


        static VolunteerController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false
            };
            client = new HttpClient(handler);

            client.BaseAddress = new Uri("http://localhost:56807/api/");
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));




        }



        // GET: Volunteer/List
        public ActionResult List()
        {
            string url = "volunteerdata/getvolunteers";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<VolunteerDto> SelectedVolunteers = response.Content.ReadAsAsync<IEnumerable<VolunteerDto>>().Result;
                Debug.WriteLine("I have grabbed " + SelectedVolunteers.Count());
                return View(SelectedVolunteers);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Volunteer/Details/5
        //public ActionResult Details(int id)
        //{
        //    ShowVolunteer ViewModel = new ShowVolunteer();
        //    string url = "volunteerdata/findvolunteer/" + id;
        //    HttpResponseMessage response = client.GetAsync(url).Result;
        //    //Can catch the status code (200 OK, 301 REDIRECT), etc.

        //    if (response.IsSuccessStatusCode)
        //    {
        //        //Volunteer goes in Data Transfer Object
        //        VolunteerDto SelectedVolunteer = response.Content.ReadAsAsync<VolunteerDto>().Result;
        //        ViewModel.volunteer = SelectedVolunteer;


        //        url = "volunteerdata/finddepartmentforvolunteer/" + id;
        //        response = client.GetAsync(url).Result;
        //        DepartmentDto SelectedDepartment = response.Content.ReadAsAsync<DepartmentDto>().Result;
        //        ViewModel.department = SelectedDepartment;

        //        return View(ViewModel);
        //    }
        //    else
        //    {
        //        return RedirectToAction("Error");
        //    }
        //}

        // GET: Volunteer/Create
        //public ActionResult Create()
        //{
        //    UpdateVolunteer ViewModel = new UpdateVolunteer();
        //    //get information about departments this volunteer could also be in
        //    string url = "departmentdata/getdepartments";
        //    HttpResponseMessage response = client.GetAsync(url).Result;
        //    IEnumerable<DepartmentDto> PotentialDepartments = response.Content.ReadAsAsync<IEnumerable<DepartmentDto>>().Result;
        //    ViewModel.alldepartments = PotentialDepartments;

        //    return View(ViewModel);
        //}

        // POST: Volunteer/Create
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Create(Volunteer VolunteerInfo)
        {
            Debug.WriteLine(VolunteerInfo.FirstName);
            string url = "volunteerdata/addvolunteer";
            Debug.WriteLine(jss.Serialize(VolunteerInfo));
            HttpContent content = new StringContent(jss.Serialize(VolunteerInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {

                int volunteerid = response.Content.ReadAsAsync<int>().Result;
                return RedirectToAction("Details", new { id = volunteerid });
            }
            else
            {
                return RedirectToAction("Error");
            }


        }

        // GET: Volunteer/Edit/8
        //public ActionResult Edit(int id)
        //{
        //    UpdateVolunteer ViewModel = new UpdateVolunteer();

        //    string url = "volunteerdata/findvolunteer/" + id;
        //    HttpResponseMessage response = client.GetAsync(url).Result;


        //    if (response.IsSuccessStatusCode)
        //    {
        //        //Put data into volunteer data transfer object
        //        VolunteerDto SelectedVolunteer = response.Content.ReadAsAsync<VolunteerDto>().Result;
        //        ViewModel.volunteer = SelectedVolunteer;

        //        //get information about departments this volunteer could also be in
        //        url = "departmentdata/getdepartments";
        //        response = client.GetAsync(url).Result;
        //        IEnumerable<DepartmentDto> PotentialDepartments = response.Content.ReadAsAsync<IEnumerable<DepartmentDto>>().Result;
        //        ViewModel.alldepartments = PotentialDepartments;

        //        return View(ViewModel);
        //    }
        //    else
        //    {
        //        return RedirectToAction("Error");
        //    }
        //}

        // POST: Volunteer/Edit/2
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(int id, Volunteer VolunteerInfo, HttpPostedFileBase VolunteerPic)
        {
            Debug.WriteLine(VolunteerInfo.FirstName);
            string url = "volunteerdata/updatevolunteer/" + id;
            Debug.WriteLine(jss.Serialize(VolunteerInfo));
            HttpContent content = new StringContent(jss.Serialize(VolunteerInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {

                //Send over image data for volunteer
                url = "volunteerdata/updatevolunteerpic/" + id;
                Debug.WriteLine("Received volunteer picture " + VolunteerPic.FileName);

                MultipartFormDataContent requestcontent = new MultipartFormDataContent();
                HttpContent imagecontent = new StreamContent(VolunteerPic.InputStream);
                requestcontent.Add(imagecontent, "VolunteerPic", VolunteerPic.FileName);
                response = client.PostAsync(url, requestcontent).Result;

                return RedirectToAction("Details", new { id = id });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Volunteer/Delete/5
        [HttpGet]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "volunteerdata/findvolunteer/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                //Put data into volunteer data transfer object
                VolunteerDto SelectedVolunteer = response.Content.ReadAsAsync<VolunteerDto>().Result;
                return View(SelectedVolunteer);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: Volunteer/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Delete(int id)
        {
            string url = "volunteerdata/deletevolunteer/" + id;

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