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
using SensenbrennerHospital.Models.ViewModels;

namespace SensenbrennerHospital.Controllers
{
    public class FaqController : Controller
    {
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client;

        static FaqController()
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
            List<ListFaq> ViewModel = new List<ListFaq>();
            string url = "FaqData/ListFaqs";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<FaqDto> FaqList = response.Content.ReadAsAsync<IEnumerable<FaqDto>>().Result;
            foreach (var item in FaqList)
            {
                url = "CategoryData/GetCategoriesForFaq/" + item.FaqID;
                response = client.GetAsync(url).Result;
                IEnumerable<CategoryDto> selectedCategories = response.Content.ReadAsAsync<IEnumerable<CategoryDto>>().Result;
                ListFaq newListFaq = new ListFaq
                {
                    isadmin = User.IsInRole("Admin"),
                    faq = item,
                    category = selectedCategories
                };
                ViewModel.Add(newListFaq);
            }
            return View(ViewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(Faq NewFaq)
        {
            string url = "FaqData/AddFaq";

            HttpContent content = new StringContent(jss.Serialize(NewFaq));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                int faqID = response.Content.ReadAsAsync<int>().Result;
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
    }
}