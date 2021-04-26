using SensenbrennerHospital.Models;
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
		// GET: Donation
		public ActionResult Index()
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



		// GET: Donation/Details/5
		public ActionResult Details(int id)
		{
			string url = "DonationData/GetDonation/" + id;
			HttpResponseMessage httpResponse = client.GetAsync(url).Result;


			if (httpResponse.IsSuccessStatusCode)
			{
				Donation selectedDonation = new Donation();
				selectedDonation = httpResponse.Content.ReadAsAsync<Donation>().Result;


				return View(selectedDonation);


			}
			else
			{
				return RedirectToAction("Error");
			}
		}


		// GET: Donation/Create
		public ActionResult Create()
		{
			return View();
		}


		// POST: Donation/Create
		[HttpPost]
		[Authorize]
		public ActionResult Create(Donation NewDonation)
		{
			string url = "DonationData/AddDonation";


			HttpContent content = new StringContent(jss.Serialize(NewDonation));
			content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
			HttpResponseMessage httpResponse = client.PostAsync(url, content).Result;
			Debug.WriteLine(jss.Serialize(NewDonation));


			if (httpResponse.IsSuccessStatusCode)
			{
				int DonationID = httpResponse.Content.ReadAsAsync<int>().Result;


				return RedirectToAction("Details", new { id = DonationID });
			}
			else
			{
				return RedirectToAction("Error");
			}
		}


		// GET: Donation/Edit/5
		[Authorize(Roles = "Admin")]
		public ActionResult Edit(int id)
		{
			string url = "DonationData/GetDonation/" + id;
			HttpResponseMessage httpResponse = client.GetAsync(url).Result;


			if (httpResponse.IsSuccessStatusCode)
			{
				Donation selectedDonation = new Donation();
				selectedDonation = httpResponse.Content.ReadAsAsync<Donation>().Result;


				return View(selectedDonation);


			}
			else
			{
				return RedirectToAction("Error");
			}
		}


		// POST: Donation/Edit/5
		[HttpPost]
		
		public ActionResult Edit(int id, Donation selectedDonation)
		{
			string url = "DonationData/UpdateDonation/" + id;
			HttpContent content = new StringContent(jss.Serialize(selectedDonation));
			content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
			HttpResponseMessage httpResponse = client.PostAsync(url, content).Result;


			//if (httpResponse.IsSuccessStatusCode)
			//{
			//	return RedirectToAction("Details", new { id = id });
			//}
			//else
			//{
			//	return RedirectToAction("Error");
			//}


			return RedirectToAction("Details", new { id = id });
		}


		// GET: Donation/Delete/5
		
		public ActionResult DeleteConfirm(int id)
		{
			string url = "DonationData/GetDonation/" + id;
			HttpResponseMessage httpResponse = client.GetAsync(url).Result;


			if (httpResponse.IsSuccessStatusCode)
			{
				Donation selectedDonation = new Donation();
				selectedDonation = httpResponse.Content.ReadAsAsync<Donation>().Result;


				return View(selectedDonation);


			}
			else
			{
				return RedirectToAction("Error");
			}
		}


		// POST: Donation/Delete/5
		[HttpPost]

		public ActionResult Delete(int id)
		{
			string url = "DonationData/DeleteDonation/" + id;
			HttpContent content = new StringContent("");
			HttpResponseMessage httpResponse = client.PostAsync(url, content).Result;


			if (httpResponse.IsSuccessStatusCode)
			{
				return RedirectToAction("Index");
			}
			else
			{
				return RedirectToAction("Error");
			}
		}




	}
}
