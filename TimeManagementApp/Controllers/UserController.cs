using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Helpers;
using TimeManagementApp.Models;
using TimeManagementApp.DTOs;

namespace TimeManagementApp.Controllers
{
	public class UserData
	{
		public string FullName { get; set; }
		public string Password { get; set; }
	}

	[RoutePrefix("api/User")]
    public class UserController : TimeAppController
    {
		[HttpGet]
		[Route("List")]
		public IEnumerable<UserDTO> GetUsersList()
		{
			return db.Users.ToArray().Select(e => new UserDTO(e));
		}

		[HttpGet]
		[Route("{id}")]
		[ResponseType(typeof(UserDTO))]
		public IHttpActionResult GetUserSummary(int id)
		{
			User user = db.Users.Find(id);
			if (user == null)
			{
				return NotFound();
			}
			return Ok(new UserDTO(user));
		}

		[HttpGet]
		[Route("Info/{login}")]
		[ResponseType(typeof(UserDTOCompany))]
		public IHttpActionResult GetUserByLogin(string login)
		{
			User user = db.Users.SingleOrDefault(e => e.AuthData.Login == login);
			if (user == null)
			{
				return NotFound();
			}
			return Ok(new UserDTOCompany(user));
		}

		[HttpGet]
		[Route("~/api/Project/{id}/Users")]
		[AuthThisProjectCompany]
		[ResponseType(typeof(UserDTOProjectTime))]
		public IHttpActionResult GetUsersByProject(int id)
		{
			Project project = db.Projects.Find(id);
			if (project == null)
			{
				return NotFound();
			}
			return Ok(project.Participants.ToArray().Select(e => new UserDTOProjectTime(e.User, id)));
		}

		[HttpPut]
		[Route("{id}")]
		[AuthThisUserOrHisCompany]
		public IHttpActionResult EditUser(int id, UserData data)
		{
			User user = db.Users.Find(id);
			if (user == null)
			{
				return NotFound();
			}

			user.FullName = data.FullName;
			db.Entry(user).State = EntityState.Modified;

			if (data.Password != null && data.Password != "")
			{
				user.AuthData.Password = Crypto.HashPassword(data.Password);
				db.Entry(user.AuthData).State = EntityState.Modified;
			}

			db.SaveChanges();
			return Ok();
		}

		[HttpGet]
		[Route("TagStatistic")]
		[AuthCompany]
		public IEnumerable<UserDTOTagsTime> TagStatistic()
		{
			return db.Users.ToArray().Select(u => new UserDTOTagsTime(u, db.RecordTags)).ToArray();
		}

		// POST api/User
		[ResponseType(typeof(UserDTO))]
		public IHttpActionResult PostUser(User user)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			db.Users.Add(user);
			db.SaveChanges();

			return CreatedAtRoute("DefaultApi", new { id = user.ID }, user);
		}

		// DELETE api/User/5
		[ResponseType(typeof(UserDTO))]
		public IHttpActionResult DeleteUser(int id)
		{
			User user = db.Users.Find(id);
			if (user == null)
			{
				return NotFound();
			}

			db.Users.Remove(user);
			db.SaveChanges();

			return Ok(new UserDTO(user));
		}

		private bool UserExists(int id)
		{
			return db.Users.Count(e => e.ID == id) > 0;
		}
    }
}
