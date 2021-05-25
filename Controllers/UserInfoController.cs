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
	public class UserInfoController : Controller
	{
		//Http Client is the proper way to connect to a webapi
		//https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httpclient?view=net-5.0


		private JavaScriptSerializer jss = new JavaScriptSerializer();
		private static readonly HttpClient client;


		/// <summary>
		/// This allows us to access a pre-defined C# HttpClient 'client' variable for sending POST and GET requests to the data access layer.
		/// </summary>
		static UserInfoController()
		{
			HttpClientHandler handler = new HttpClientHandler()
			{
				AllowAutoRedirect = false,
				//cookies are manually set in RequestHeader
				UseCookies = false
			};

			client = new HttpClient(handler);
			//change this to match your own local port number
			client.BaseAddress = new Uri("https://localhost:44336/api/");
			client.DefaultRequestHeaders.Accept.Add(
			new MediaTypeWithQualityHeaderValue("application/json"));
		}


		/// <summary>
		/// Grabs the authentication credentials which are sent to the Controller.
		/// This is NOT considered a proper authentication technique for the WebAPI. It piggybacks the existing authentication set up in the template for Individual User Accounts. Considering the existing scope and complexity of the course, it works for now.
		/// 
		/// Here is a descriptive article which walks through the process of setting up authorization/authentication directly.
		/// https://docs.microsoft.com/en-us/aspnet/web-api/overview/security/individual-accounts-in-web-api
		/// </summary>
		private void GetApplicationCookie()
		{
			string token = "";
			//HTTP client is set up to be reused, otherwise it will exhaust server resources.
			//This is a bit dangerous because a previously authenticated cookie could be cached for
			//a follow-up request from someone else. Reset cookies in HTTP client before grabbing a new one.
			client.DefaultRequestHeaders.Remove("Cookie");
			if (!User.Identity.IsAuthenticated) return;


			HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies.Get(".AspNet.ApplicationCookie");
			if (cookie != null) token = cookie.Value;


			//collect token as it is submitted to the controller
			//use it to pass along to the WebAPI.
			Debug.WriteLine("Token Submitted is : " + token);
			if (token != "") client.DefaultRequestHeaders.Add("Cookie", ".AspNet.ApplicationCookie=" + token);


			return;
		}




		// GET: UserInfo/List?{PageNum}
		// If the page number is not included, set it to 0
		public ActionResult List(int PageNum = 0)
		{

			ListUser ViewModel = new ListUser();
			ViewModel.isadmin = User.IsInRole("Admin");



			// Grab all users
			string url = "userdata/getusers";
			// Send off an HTTP request
			// GET : /api/userdata/getusers
			// Retrieve response
			HttpResponseMessage response = client.GetAsync(url).Result;
			// If the response is a success, proceed
			if (response.IsSuccessStatusCode)
			{
				// Fetch the response content into IEnumerable<UserDto>
				IEnumerable<UserInfoDto> SelectedUsers = response.Content.ReadAsAsync<IEnumerable<UserInfoDto>>().Result;


				// -- Start of Pagination Algorithm --


				// Find the total number of users
				int UserCount = SelectedUsers.Count();
				// Number of users to display per page
				int PerPage = 8;
				// Determines the maximum number of pages (rounded up), assuming a page 0 start.
				int MaxPage = (int)Math.Ceiling((decimal)UserCount / PerPage) - 1;


				// Lower boundary for Max Page
				if (MaxPage < 0) MaxPage = 0;
				// Lower boundary for Page Number
				if (PageNum < 0) PageNum = 0;
				// Upper Bound for Page Number
				if (PageNum > MaxPage) PageNum = MaxPage;


				// The Record Index of our Page Start
				int StartIndex = PerPage * PageNum;


				//Helps us generate the HTML which shows "Page 1 of ..." on the list view
				ViewData["PageNum"] = PageNum;
				ViewData["PageSummary"] = " " + (PageNum + 1) + " of " + (MaxPage + 1) + " ";


				// -- End of Pagination Algorithm --




				// Send back another request to get users, this time according to our paginated logic rules
				// GET api/userdata/getuserspage/{startindex}/{perpage}
				url = "userdata/getuserspage/" + StartIndex + "/" + PerPage;
				response = client.GetAsync(url).Result;


				// Retrieve the response of the HTTP Request
				IEnumerable<UserInfoDto> SelectedUsersPage = response.Content.ReadAsAsync<IEnumerable<UserInfoDto>>().Result;


				ViewModel.users = SelectedUsersPage;


				//Return the paginated of users instead of the entire list
				return View(ViewModel);


			}
			else
			{
				// If we reach here something went wrong with our list algorithm
				return RedirectToAction("Error");
			}



		}


		// GET: UserInfo/Details/5
		public ActionResult Details(int id)
		{

			ShowUser ViewModel = new ShowUser();


			//Pass along to the view information about who is logged in
			ViewModel.isadmin = User.IsInRole("Admin");





			string url = "userdata/finduser/" + id;
			HttpResponseMessage response = client.GetAsync(url).Result;
			//Can catch the status code (200 OK, 301 REDIRECT), etc.
			//Debug.WriteLine(response.StatusCode);
			if (response.IsSuccessStatusCode)
			{
				//Put data into user data transfer object
				UserInfoDto SelectedUser = response.Content.ReadAsAsync<UserInfoDto>().Result;
				ViewModel.user = SelectedUser;



				url = "userdata/finddonationforuser/" + id;
				response = client.GetAsync(url).Result;
				DonationDto SelectedDonation = response.Content.ReadAsAsync<DonationDto>().Result;
				ViewModel.donation = SelectedDonation;


				return View(ViewModel);
			}
			else
			{
				return RedirectToAction("Error");
			}
		}


		// GET: User/Create
		// only administrators get to this page
		[Authorize(Roles = "Admin")]
		public ActionResult Create()
		{
			UpdateUser ViewModel = new UpdateUser();
			//get information about donations this user COULD play for.
			string url = "donationdata/getdonations";
			HttpResponseMessage response = client.GetAsync(url).Result;
			IEnumerable<DonationDto> PotentialDonations = response.Content.ReadAsAsync<IEnumerable<DonationDto>>().Result;
			ViewModel.alldonations = PotentialDonations;


			return View(ViewModel);
		}


		// POST: User/Create
		[HttpPost]
		[ValidateAntiForgeryToken()]
		[Authorize(Roles = "Admin")]
		public ActionResult Create(UserInfo UserInfo)
		{
			//pass along authentication credential in http request
			GetApplicationCookie();


			//Debug.WriteLine(UserInfo.FirstName);
			//Debug.WriteLine(jss.Serialize(UserInfo));
			string url = "userdata/adduser";
			HttpContent content = new StringContent(jss.Serialize(UserInfo));
			content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
			HttpResponseMessage response = client.PostAsync(url, content).Result;


			if (response.IsSuccessStatusCode)
			{
				int userid = response.Content.ReadAsAsync<int>().Result;
				return RedirectToAction("Details", new { id = userid });
			}
			else
			{
				return RedirectToAction("Error");
			}
		}


		// GET: UserInfo/Edit/5
		[Authorize(Roles = "Admin")]
		public ActionResult Edit(int id)
		{
			UpdateUser ViewModel = new UpdateUser();


			string url = "userinfodata/finduser/" + id;
			HttpResponseMessage response = client.GetAsync(url).Result;
			//Can catch the status code (200 OK, 301 REDIRECT), etc.
			//Debug.WriteLine(response.StatusCode);
			if (response.IsSuccessStatusCode)
			{
				//Put data into user data transfer object
				UserInfoDto SelectedUser = response.Content.ReadAsAsync<UserInfoDto>().Result;
				ViewModel.user = SelectedUser;


				//get information about teams this user COULD play for.
				url = "teamdata/getteams";
				response = client.GetAsync(url).Result;
				IEnumerable<DonationDto> PotentialDonations = response.Content.ReadAsAsync<IEnumerable<DonationDto>>().Result;
				ViewModel.alldonations = PotentialDonations;


				return View(ViewModel);
			}
			else
			{
				return RedirectToAction("Error");
			}
		}


		// POST: UserIfo/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken()]
		[Authorize(Roles = "Admin")]
		public ActionResult Edit(int id, UserInfo UserInfo, HttpPostedFileBase UserPic)
		{
			//pass along authentication credential in http request
			GetApplicationCookie();


			//Debug.WriteLine(UserInfo.UserFirstName);
			string url = "userdata/updateuser/" + id;
			Debug.WriteLine(jss.Serialize(UserInfo));
			HttpContent content = new StringContent(jss.Serialize(UserInfo));
			content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
			HttpResponseMessage response = client.PostAsync(url, content).Result;
			//Debug.WriteLine(response.StatusCode);
			if (response.IsSuccessStatusCode)
			{

				return RedirectToAction("Details", new { id = id });
			}
			else
			{
				return RedirectToAction("Error");
			}
		}


		// GET: UserInfo/Delete/5
		[HttpGet]
		[Authorize(Roles = "Admin")]
		public ActionResult DeleteConfirm(int id)
		{
			string url = "userdata/finduser/" + id;
			HttpResponseMessage response = client.GetAsync(url).Result;
			//Can catch the status code (200 OK, 301 REDIRECT), etc.
			//Debug.WriteLine(response.StatusCode);
			if (response.IsSuccessStatusCode)
			{
				//Put data into user data transfer object
				UserInfoDto SelectedUser = response.Content.ReadAsAsync<UserInfoDto>().Result;
				return View(SelectedUser);
			}
			else
			{
				return RedirectToAction("Error");
			}
		}


		// POST: UserInfo/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken()]
		[Authorize(Roles = "Admin")]
		public ActionResult Delete(int id)
		{
			//pass along authentication credential in http request
			GetApplicationCookie();


			string url = "userinfodata/deleteuser/" + id;
			//post body is empty
			HttpContent content = new StringContent("");
			HttpResponseMessage response = client.PostAsync(url, content).Result;
			//Can catch the status code (200 OK, 301 REDIRECT), etc.
			//Debug.WriteLine(response.StatusCode);
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

