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
    public class AppointmentBookingController : Controller
    {
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client;
        static AppointmentBookingController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false
            };
            client = new HttpClient(handler);
            client.BaseAddress = new Uri("https://localhost:44336/api/");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        // Homepage for this table shows all the bookings, this can be adjusted later
        // GET: AppointmentBooking
        public ActionResult Index()
        {
            List<ListAppointment> ViewModel = new List<ListAppointment> { };
            string url = "AppointmentBookingData/ListAppointmentBookings";
            HttpResponseMessage httpResponse = client.GetAsync(url).Result;

            if (httpResponse.IsSuccessStatusCode)
            {
                IEnumerable<AppointmentBookingDto> BookingList = httpResponse.Content.ReadAsAsync<IEnumerable<AppointmentBookingDto>>().Result;
                foreach(var item in BookingList)
                {
                    url = "DoctorData/GetDoctor/" + item.DoctorID;
                    httpResponse = client.GetAsync(url).Result;

                    ListAppointment listAppointment = new ListAppointment
                    {
                        appointmentBookingDto = item,
                        isadmin = User.IsInRole("Admin"),
                        doctorDto = httpResponse.Content.ReadAsAsync<DoctorDTO>().Result
                    };
                    ViewModel.Add(listAppointment);
                }
                Debug.WriteLine(ViewModel);
                return View(ViewModel);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // Shows detail for a certain entry
        // GET: AppointmentBooking/Details/5
        public ActionResult Details(int id)
        {
            string url = "AppointmentBookingData/GetAppointmentBooking/" + id;
            HttpResponseMessage httpResponse = client.GetAsync(url).Result;

            if (httpResponse.IsSuccessStatusCode)
            {
                AppointmentBookingDto selectedBooking = new AppointmentBookingDto();
                selectedBooking = httpResponse.Content.ReadAsAsync<AppointmentBookingDto>().Result;

                url = "DoctorData/GetDoctor/" + selectedBooking.DoctorID;
                httpResponse = client.GetAsync(url).Result;
                ListAppointment ViewModel = new ListAppointment
                {
                    isadmin = User.IsInRole("Admin"),
                    appointmentBookingDto = selectedBooking,
                    doctorDto = httpResponse.Content.ReadAsAsync<DoctorDTO>().Result
                };
                return View(ViewModel);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: AppointmentBooking/Create
        public ActionResult Create()
        {
            AppointmentBookingDto appointmentBookingDto = new AppointmentBookingDto();
            string URL = "DoctorData/GetListOfDoctors";
            HttpResponseMessage httpResponse = client.GetAsync(URL).Result;

            if (httpResponse.IsSuccessStatusCode)
            {
                List<SelectListItem> doctorSelectList = new List<SelectListItem>();
                IEnumerable<DoctorDTO> doctorList = httpResponse.Content.ReadAsAsync<IEnumerable<DoctorDTO>>().Result;

                foreach (var d in doctorList)
                {
                    doctorSelectList.Add(new SelectListItem
                    {
                        Text = "Dr." + d.LastName,
                        Value = d.DoctorID.ToString()
                    });
                }
                Debug.WriteLine(doctorSelectList);
                appointmentBookingDto.doctorSelectList = doctorSelectList;
                return View(appointmentBookingDto);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: AppointmentBooking/Create
        [HttpPost]
        public ActionResult Create(AppointmentBooking NewAppointmentBooking)
        {
            NewAppointmentBooking.Confirmation = "0";
            string url = "AppointmentBookingData/AddAppointmentBooking";

            HttpContent content = new StringContent(jss.Serialize(NewAppointmentBooking));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            Debug.WriteLine(jss.Serialize(NewAppointmentBooking));
            HttpResponseMessage httpResponse = client.PostAsync(url, content).Result;

            if (httpResponse.IsSuccessStatusCode)
            {
                int BookingID = httpResponse.Content.ReadAsAsync<int>().Result;

                return RedirectToAction("Details", new { id = BookingID });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: AppointmentBooking/Edit/5
        [HttpGet]
        public ActionResult Edit(int id)
        {
            string url = "AppointmentBookingData/GetAppointmentBooking/" + id;
            HttpResponseMessage httpResponse = client.GetAsync(url).Result;

            if (httpResponse.IsSuccessStatusCode)
            {
                AppointmentBookingDto selectedBooking = new AppointmentBookingDto();
                selectedBooking = httpResponse.Content.ReadAsAsync<AppointmentBookingDto>().Result;

                url = "DoctorData/GetListOfDoctors";
                httpResponse = client.GetAsync(url).Result;
                List<SelectListItem> doctorSelectList = new List<SelectListItem>();
                IEnumerable<DoctorDTO> doctorList = httpResponse.Content.ReadAsAsync<IEnumerable<DoctorDTO>>().Result;

                foreach (var d in doctorList)
                {
                    doctorSelectList.Add(new SelectListItem
                    {
                        Text = "Dr." + d.LastName,
                        Value = d.DoctorID.ToString()
                    });
                }
                selectedBooking.doctorSelectList = doctorSelectList;

                return View(selectedBooking);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: AppointmentBooking/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, AppointmentBooking selectedBooking)
        {
            string url = "AppointmentBookingData/UpdateAppointmentBooking/" + id;
            HttpContent content = new StringContent(jss.Serialize(selectedBooking));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage httpResponse = client.PostAsync(url, content).Result;

            if (httpResponse.IsSuccessStatusCode)
            {
                return RedirectToAction("Details", new { id = id });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: AppointmentBooking/DeleteConfirm/5
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "AppointmentBookingData/GetAppointmentBooking/" + id;
            HttpResponseMessage httpResponse = client.GetAsync(url).Result;

            if (httpResponse.IsSuccessStatusCode)
            {
                AppointmentBookingDto selectedBooking = new AppointmentBookingDto();
                selectedBooking = httpResponse.Content.ReadAsAsync<AppointmentBookingDto>().Result;

                url = "DoctorData/GetDoctor/" + selectedBooking.DoctorID;
                httpResponse = client.GetAsync(url).Result;
                ListAppointment ViewModel = new ListAppointment
                {
                    isadmin = User.IsInRole("Admin"),
                    appointmentBookingDto = selectedBooking,
                    doctorDto = httpResponse.Content.ReadAsAsync<DoctorDTO>().Result
                };

                return View(ViewModel);

            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: AppointmentBooking/Delete/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            string url = "AppointmentBookingData/DeleteAppointmentBooking/" + id;
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
