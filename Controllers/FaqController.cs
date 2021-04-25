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

        //GET: Faq/List
        [HttpGet]
       public ActionResult List()
        {
            List<ListFaq> ViewModel = new List<ListFaq>();
            string url = "FaqData/ListFaqs";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<FaqDto> FaqList = response.Content.ReadAsAsync<IEnumerable<FaqDto>>().Result;

            url = "CategoryData/GetCategories";
            response = client.GetAsync(url).Result;
            IEnumerable<CategoryDto> categoryList = response.Content.ReadAsAsync<IEnumerable<CategoryDto>>().Result;

            foreach (var item in FaqList)
            {
                url = "CategoryData/FindCategory/" + item.CategoryID;
                response = client.GetAsync(url).Result;
                CategoryDto selectedCategory = response.Content.ReadAsAsync<CategoryDto>().Result;
                ListFaq newListFaq = new ListFaq
                {
                    isadmin = User.IsInRole("Admin"),
                    faq = item,
                    category = selectedCategory,
                    listOfCategories = categoryList
                };
                ViewModel.Add(newListFaq);
            }
            return View(ViewModel);
        }

        //GET: Faq/Create
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            CreateFaq ViewModel = new CreateFaq();

            string url = "CategoryData/GetCategories";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<CategoryDto> PotentialCategory = response.Content.ReadAsAsync<IEnumerable<CategoryDto>>().Result;
            ViewModel.allCategories = PotentialCategory;

            return View(ViewModel);
        }

        //POST: Faq/Create
        [HttpPost]
        [ValidateAntiForgeryToken()]
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
                //Change this to details when controller is ready
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        //GET: Faq/Update/4
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Update(int id)
        {
            CreateFaq ViewModel = new CreateFaq();

            string url = "FaqData/FindFaq/" + id;

            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                FaqDto selectedFaq = response.Content.ReadAsAsync<FaqDto>().Result;
                ViewModel.faq = selectedFaq;

                url = "CategoryData/GetCategories";
                response = client.GetAsync(url).Result;
                IEnumerable<CategoryDto> PotentialCategories = response.Content.ReadAsAsync<IEnumerable<CategoryDto>>().Result;
                ViewModel.allCategories = PotentialCategories;

                return View(ViewModel);
            }
            else
            {
                return RedirectToAction("Error");
            }

        }

        //POST: Faq/Update/4
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [Authorize(Roles = "Admin")]
        public ActionResult Update(int id, Faq FaqInfo)
        {
            string url = "FaqData/UpdateFaq/" + id;

            HttpContent content = new StringContent(jss.Serialize(FaqInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
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

        //GET : Faq/DeleteConfirm/2
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "FaqData/FindFaq/" + id;

            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                FaqDto SelectedFaq = response.Content.ReadAsAsync<FaqDto>().Result;
                return View(SelectedFaq);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        //POST: Faq/Delete/2
        public ActionResult Delete(int id)
        {
            string url = "FaqData/DeleteFaq/" + id;
            //Body is empty
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