using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using SensenbrennerHospital.Models;
using System.Diagnostics;


namespace SensenbrennerHospital.Controllers
{
	public class UserDataController : ApiController
	{
		//This variable is our database access point
		private ApplicationDbContext db = new ApplicationDbContext();


		//This code is mostly scaffolded from the base models and database context
		//New > WebAPIController with Entity Framework Read/Write Actions
		//Choose model "User"
		//Choose context "ApplicationDbContext"
		//Note: The base scaffolded code needs many improvements for a fully
		//functioning MVP.


		// GET: api/UserInfoData/GetUsers
		// TODO: Searching Logic?


		//Authorize annotation will block requests unless user is authorized
		//authorization process checks for valid cookies in request
		[Authorize]
		public IEnumerable<UserInfoDto> GetUsers()
		{
			List<UserInfo> UserInfos = db.UserInfos.ToList();
			List<UserInfoDto> UserInfoDtos = new List<UserInfoDto> { };


			//Here you can choose which information is exposed to the API
			foreach (var User in UserInfos)
			{
				UserInfoDto NewUser = new UserInfoDto
				{
					UserID = User.UserID,
					FirstName = User.FirstName,
					LastName = User.LastName,
					StreetNumber = User.StreetNumber,
					Address = User.Address,
					PostalCode= User.PostalCode,
					PhoneNumber = User.PhoneNumber
					

				};
				UserInfoDtos.Add(NewUser);
			}


			return UserInfoDtos;
		}


		// GET: api/UserInfoData/FindUser/5
		[ResponseType(typeof(UserInfoDto))]
		[HttpGet]
		public IHttpActionResult FindUser(int id)
		{
			//Find the data
			UserInfo User = db.UserInfos.Find(id);
			//if not found, return 404 status code.
			if (User == null)
			{
				return NotFound();
			}


			//put into a 'friendly object format'
			UserInfoDto UserInfoDto = new UserInfoDto
			{
				UserID = User.UserID,
				FirstName = User.FirstName,
				LastName = User.LastName,
				StreetNumber = User.StreetNumber,
				Address = User.Address,
				PostalCode = User.PostalCode,
				PhoneNumber = User.PhoneNumber
			};




			//pass along data as 200 status code OK response
			return Ok(UserInfoDto);
		}


		// POST: api/UserInfoData/UpdateUser/5
		// FORM DATA: User JSON Object
		[ResponseType(typeof(void))]
		[HttpPost]
		public IHttpActionResult UpdateUser(int id, [FromBody] UserInfo user)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}


			if (id != user.UserID)
			{
				return BadRequest();
			}


			db.Entry(user).State = EntityState.Modified;


			try
			{
				db.SaveChanges();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!UserInfoExists(id))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}


			return StatusCode(HttpStatusCode.NoContent);
		}


		// POST: api/UserInfos/AddUser
		// FORM DATA: User JSON Object
		[ResponseType(typeof(UserInfo))]
		[HttpPost]
		public IHttpActionResult AddUser([FromBody] UserInfo user)
		{
			//Will Validate according to data annotations specified on model
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}


			db.UserInfos.Add(user);
			db.SaveChanges();


			return CreatedAtRoute("DefaultApi", new { id = user.UserID }, user);
		}


		// POST: api/UserInfos/DeleteUser/5
		[HttpPost]
		public IHttpActionResult DeleteUser(int id)
		{
			UserInfo user = db.UserInfos.Find(id);
			if (user == null)
			{
				return NotFound();
			}


			db.UserInfos.Remove(user);
			db.SaveChanges();


			return Ok();
		}


		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				db.Dispose();
			}
			base.Dispose(disposing);
		}


		private bool UserInfoExists(int id)
		{
			return db.UserInfos.Count(e => e.UserID == id) > 0;
		}
	}


}


