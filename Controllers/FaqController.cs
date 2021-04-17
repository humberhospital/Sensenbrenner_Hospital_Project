using System;
using System.Collections.Generic;
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
                    faq = item,
                    category = selectedCategories
                };
                ViewModel.Add(newListFaq);
            }
            return View(ViewModel);
        }
    }
}