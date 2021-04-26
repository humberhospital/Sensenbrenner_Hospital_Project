using SensenbrennerHospital.Models;
using SensenbrennerHospital.Models.ViewModels;
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
    public class CategoryController : Controller
    {
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client;

        static CategoryController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false
            };
            client = new HttpClient(handler);
            client.BaseAddress = new Uri("https://localhost:44336/api/");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        //GET: Category/List
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult List()
        {
            string url = "CategoryData/ListCategories";
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                IEnumerable<CategoryDto> CategoryList = response.Content.ReadAsAsync<IEnumerable<CategoryDto>>().Result;
                return View(CategoryList);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }


        //GET: Category/Create
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        //POST: Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(Category NewCategory)
        {
            string url = "CategoryData/AddCategory";

            HttpContent content = new StringContent(jss.Serialize(NewCategory));
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

        //GET: Category/DeleteConfirm/1
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirm(int id)
        {
            DeleteCategory ViewModel = new DeleteCategory();

            string url = "CategoryData/FindCategory/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                CategoryDto category = response.Content.ReadAsAsync<CategoryDto>().Result;
                ViewModel.category = category;

                url = "FaqData/GetFaqsByCategoryId/" + category.CategoryID;
                response = client.GetAsync(url).Result;
                IEnumerable<FaqDto> faqList = response.Content.ReadAsAsync<IEnumerable<FaqDto>>().Result;
                ViewModel.faqList = faqList;

                return View(ViewModel);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        //POST: Category/Delete/1
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            string url = "FaqData/DeleteFaqsByCategoryId/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                url = "CategoryData/DeleteCategory/" + id;
                response = client.GetAsync(url).Result;
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
            
        }

        //GET: Category/Update/2
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Update(int id)
        {
            string url = "CategoryData/FindCategory/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            
            if (response.IsSuccessStatusCode)
            {
                CategoryDto SelectedCategory = response.Content.ReadAsAsync<CategoryDto>().Result;
                return View(SelectedCategory);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        //POST: Category/Update/2
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [Authorize(Roles = "Admin")]
        public ActionResult Update(int id, Category CategoryInfo)
        {
            string url = "CategoryData/UpdateCategory/" + id;

            HttpContent content = new StringContent(jss.Serialize(CategoryInfo));
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

        public ActionResult Error()
        {
            return View();
        }
    }
}