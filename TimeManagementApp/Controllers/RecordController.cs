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
using TimeManagementApp.Models;
using TimeManagementApp.DTOs;

namespace TimeManagementApp.Controllers
{
	[RoutePrefix("api/Record")]
    public class RecordController : TimeAppController
    {
		[HttpGet]
		[Route("{id}")]
		[ResponseType(typeof(RecordDTO))]
		public IHttpActionResult GetRecordSummary(int id)
		{
			Record record = db.Records.Find(id);
			if (record == null)
			{
				return NotFound();
			}

			return Ok(new RecordDTO(record));
		}

		[HttpGet]
		[Route("~/api/Project/{id}/User/Me/Records/{start}/{count}")]
		[AuthThisProjectAssociate]
		[ResponseType(typeof(IEnumerable<RecordDTO>))]
		public IHttpActionResult GetMyRecordsByProject(int id, int start, int count)
		{
			Project project = db.Projects.Find(id);
			if (project == null)
			{
				return NotFound();
			}
			var records = project.Records
				.Where(r => r.UserID == getCurrentUser().ID)
				.OrderByDescending(r => r.BeginTime)
				.Skip(start).Take(count)
				.ToArray();
			return Ok(records.Select(e => new RecordDTO(e)));
		}

		[HttpGet]
		[Route("~/api/Project/{id}/User/Me/Records/{year}/{month}/{day}/{start}/{count}")]
		[AuthThisProjectAssociate]
		[ResponseType(typeof(IEnumerable<RecordDTO>))]
		public IHttpActionResult GetMyRecordsByProjectWithDate(int id, int year, int month, int day, int start, int count)
		{
			Project project = db.Projects.Find(id);
			if (project == null)
			{
				return NotFound();
			}
			var records = project.Records
				.Where(r => r.UserID == getCurrentUser().ID)
				.OrderByDescending(r => r.BeginTime)
				.Where(e => e.BeginTime.Year == year && e.BeginTime.Month == month);
			if (day != 0)
				records = records.Where(e => e.BeginTime.Day == day);
			var result = records.Skip(start).Take(count).ToArray();
			return Ok(result.Select(e => new RecordDTO(e)));
		}

		[HttpGet]
		[Route("~/api/Project/{id}/User/{user:int}/Records/{start}/{count}")]
		[AuthThisProjectCompany]
		[ResponseType(typeof(IEnumerable<RecordDTO>))]
		public IHttpActionResult GetRecordsByProject(int id, int user, int start, int count)
		{
			Project project = db.Projects.Find(id);
			if (project == null)
			{
				return NotFound();
			}
			var records = project.Records
				.Where(r => r.UserID == user)
				.OrderByDescending(r => r.BeginTime)
				.Skip(start).Take(count)
				.ToArray();
			return Ok(records.Select(e => new RecordDTO(e)));
		}

		[HttpGet]
		[Route("~/api/Project/{id}/User/{user:int}/Records/{year}/{month}/{day}/{start}/{count}")]
		[AuthThisProjectCompany]
		[ResponseType(typeof(IEnumerable<RecordDTO>))]
		public IHttpActionResult GetRecordsByProjectWithDate(int id, int user, int year, int month, int day, int start, int count)
		{
			Project project = db.Projects.Find(id);
			if (project == null)
			{
				return NotFound();
			}
			var records = project.Records
				.Where(r => r.UserID == user)
				.OrderByDescending(r => r.BeginTime)
				.Where(e => e.BeginTime.Year == year && e.BeginTime.Month == month);
			if (day != 0)
				records = records.Where(e => e.BeginTime.Day == day);
			var result = records.Skip(start).Take(count).ToArray();
			return Ok(result.Select(e => new RecordDTO(e)));
		}

		[HttpGet]
		[Route("~/api/Project/{id}/User/All/Records/{start}/{count}")]
		[AuthThisProjectCompany]
		[ResponseType(typeof(IEnumerable<RecordDTOUser>))]
		public IHttpActionResult GetAllRecordsByProject(int id, int start, int count)
		{
			Project project = db.Projects.Find(id);
			if (project == null)
			{
				return NotFound();
			}
			var records = project.Records
				.OrderByDescending(r => r.BeginTime)
				.Skip(start).Take(count)
				.ToArray();
			return Ok(records.Select(e => new RecordDTOUser(e)));
		}

		[HttpGet]
		[Route("~/api/Project/{id}/User/All/Records/{year}/{month}/{day}/{start}/{count}")]
		[AuthThisProjectCompany]
		[ResponseType(typeof(IEnumerable<RecordDTOUser>))]
		public IHttpActionResult GetAlRecordsByProjectWidthDate(int id, int year, int month, int day, int start, int count)
		{
			Project project = db.Projects.Find(id);
			if (project == null)
			{
				return NotFound();
			}
			var records = project.Records
				.OrderByDescending(r => r.BeginTime)
				.Where(e => e.BeginTime.Year == year && e.BeginTime.Month == month);
			if (day != 0)
				records = records.Where(e => e.BeginTime.Day == day);
			var result = records.Skip(start).Take(count).ToArray();
			return Ok(result.Select(e => new RecordDTOUser(e)));
		}

		[HttpGet]
		[Route("~/api/Project/{id}/User/Me/Records/Days")]
		[AuthThisProjectAssociate]
		[ResponseType(typeof(IEnumerable<int>))]
		public IHttpActionResult GetMyRecordsDatesByProject(int id, int currentMonth, int currentYear)
		{
			Project project = db.Projects.Find(id);
			if (project == null)
			{
				return NotFound();
			}
			var days = project.Records
				.Where(r => r.UserID == getCurrentUser().ID)
				.Where(e => e.BeginTime.Month == currentMonth && e.BeginTime.Year == currentYear)
				.Select(e => e.BeginTime.Day);
			return Ok(days);
		}

		[HttpGet]
		[Route("~/api/Project/{id}/User/{user:int}/Records/Days")]
		[AuthThisProjectCompany]
		[ResponseType(typeof(IEnumerable<int>))]
		public IHttpActionResult GetRecordsDatesByProject(int id, int user, int currentMonth, int currentYear)
		{
			Project project = db.Projects.Find(id);
			if (project == null)
			{
				return NotFound();
			}
			var days = project.Records
				.Where(r => r.UserID == user)
				.Where(e => e.BeginTime.Month == currentMonth && e.BeginTime.Year == currentYear)
				.Select(e => e.BeginTime.Day);
			return Ok(days);
		}

		[HttpGet]
		[Route("~/api/Project/{id}/User/All/Records/Days")]
		[AuthThisProjectCompany]
		[ResponseType(typeof(IEnumerable<int>))]
		public IHttpActionResult GetAllRecordsDatesByProject(int id, int currentMonth, int currentYear)
		{
			Project project = db.Projects.Find(id);
			if (project == null)
			{
				return NotFound();
			}
			var days = project.Records
				.Where(e => e.BeginTime.Month == currentMonth && e.BeginTime.Year == currentYear)
				.Select(e => e.BeginTime.Day);
			return Ok(days);
		}

		[HttpGet]
		[Route("~/api/Project/{id}/User/Me/Records/Days/Limits")]
		[AuthThisProjectAssociate]
		public IHttpActionResult GetMyRecordsDatesLimitsByProject(int id)
		{
			Project project = db.Projects.Find(id);
			if (project == null)
			{
				return NotFound();
			}
			var min = project.Records.Where(r => r.UserID == getCurrentUser().ID).Min(e => e.BeginTime);
			var max = project.Records.Where(r => r.UserID == getCurrentUser().ID).Max(e => e.EndTime);
			return Ok(new {
				min = min,
				max = max
			});
		}

		[HttpGet]
		[Route("~/api/Project/{id}/User/{user:int}/Records/Days/Limits")]
		[AuthThisProjectAssociate]
		public IHttpActionResult GetRecordsDatesLimitsByProject(int id, int user)
		{
			Project project = db.Projects.Find(id);
			if (project == null)
			{
				return NotFound();
			}
			var min = project.Records.Where(r => r.UserID == user).Min(e => e.BeginTime);
			var max = project.Records.Where(r => r.UserID == user).Max(e => e.EndTime);
			return Ok(new
			{
				min = min,
				max = max
			});
		}

		[HttpGet]
		[Route("~/api/Project/{id}/User/All/Records/Days/Limits")]
		[AuthThisProjectCompany]
		public IHttpActionResult GetAllRecordsDatesLimitsByProject(int id)
		{
			Project project = db.Projects.Find(id);
			if (project == null)
			{
				return NotFound();
			}
			var min = project.Records.Min(e => e.BeginTime);
			var max = project.Records.Max(e => e.EndTime);
			return Ok(new
			{
				min = min,
				max = max
			});
		}

		[HttpPut]
		[Route("{id}/Finish")]
		[ResponseType(typeof(RecordDTO))]
		public IHttpActionResult StopRecording(int id)
		{
			Record record = db.Records.Find(id);
			if (record == null)
			{
				return NotFound();
			}
			record.Status = RecordStatus.Complete;
			record.EndTime = DateTime.UtcNow;
			db.Entry(record).State = EntityState.Modified;

			db.SaveChanges();

			return Ok(new RecordDTO(record));
		}

		// POST api/Record
		[HttpPost]
		[Route("")]
		[ResponseType(typeof(RecordDTO))]
		public IHttpActionResult CreateRecord(Record record)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			db.Records.Add(record);
			db.SaveChanges();

			record.Tag = db.RecordTags.Find(record.TagID);
			return CreatedAtRoute("DefaultApi", new { controller = "Record", id = record.ID }, new RecordDTO(record));
		}

		// DELETE api/Record/5
		[ResponseType(typeof(RecordDTO))]
		public IHttpActionResult DeleteRecord(int id)
		{
			Record record = db.Records.Find(id);
			if (record == null)
			{
				return NotFound();
			}

			db.Records.Remove(record);
			db.SaveChanges();

			return Ok(new RecordDTO(record));
		}

		private bool RecordExists(int id)
		{
			return db.Records.Count(e => e.ID == id) > 0;
		}
    }
}
