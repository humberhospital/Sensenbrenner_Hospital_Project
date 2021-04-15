using SensenbrennerHospital.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http.Description;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace SensenbrennerHospital.Controllers
{
    public class DonationController : Controller
    {
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client;

        static DonationController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false
            };
            client = new HttpClient(handler);
            client.BaseAddress = new Uri("https://localhost:44336/api/");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public ActionResult List()
        {
            string url = "DonationData/GetDonations";
            HttpResponseMessage httpResponse = client.GetAsync(url).Result;

            if (httpResponse.IsSuccessStatusCode)
            {
                IEnumerable<DonationDto> DonationList = httpResponse.Content.ReadAsAsync<IEnumerable<DonationDto>>().Result;
                return View(DonationList);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        [HttpGet]
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult Create(Donation NewDonation)
        {
            string url = "DonationData/AddDonation";

            HttpContent content = new StringContent(jss.Serialize(NewDonation));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage httpResponse = client.PostAsync(url, content).Result;
            //Debug.WriteLine(jss.Serialize(NewDepartment));
            if (httpResponse.IsSuccessStatusCode)
            {
                int DonationID = httpResponse.Content.ReadAsAsync<int>().Result;
                return RedirectToAction("DonationList");
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
            string url = "DonationData/GetDonation/" + id;
            HttpResponseMessage httpResponse = client.GetAsync(url).Result;

            if (httpResponse.IsSuccessStatusCode)
            {
                DonationDto donation = new DonationDto();
                donation = httpResponse.Content.ReadAsAsync<DonationDto>().Result;
                return View(donation);
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
            string url = "DonationtData/DeleteDonation/" + id;
            HttpContent content = new StringContent("");
            HttpResponseMessage httpResponse = client.PostAsync(url, content).Result;
            if (httpResponse.IsSuccessStatusCode)
            {
                return RedirectToAction("DonationList");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        [HttpGet]
        public ActionResult DonationList()
        {
            string url = "DonationData/GetListOfDonations";
            HttpResponseMessage httpResponse = client.GetAsync(url).Result;

            if (httpResponse.IsSuccessStatusCode)
            {
                IEnumerable<DonationDto> DonationList = httpResponse.Content.ReadAsAsync<IEnumerable<DonationDto>>().Result;
                return View(DonationList);
            }
            else
            {
                return null;
            }
        }
    }
}