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
	[RoutePrefix("api/Project")]
    public class ProjectController : TimeAppController
    {
		[HttpGet]
		[Route("{id}")]
		[ResponseType(typeof(CompanyDTO))]
		public IHttpActionResult GetProjectSummary(int id)
		{
			Project project = db.Projects.Find(id);
			if (project == null)
			{
				return NotFound();
			}

			return Ok(new ProjectDTO(project));
		}

		[HttpGet]
		[Route("~/api/Company/{id}/Projects")]
		[AuthThisCompany]
		[ResponseType(typeof(IEnumerable<ProjectDTO>))]
		public IHttpActionResult GetProjectsByCompany(int id)
		{
			Company company = db.Companies.Find(id);
			if (company == null)
			{
				return NotFound();
			}
			return Ok(company.Projects.ToArray().Select(e => new ProjectDTO(e)));
		}

		[HttpGet]
		[Route("~/api/User/{id}/Projects")]
		[AuthThisUser]
		[ResponseType(typeof(IEnumerable<ProjectDTOUserTime>))]
		public IHttpActionResult GetProjectsByUser(int id)
		{
			User user = db.Users.Find(id);
			if (user == null)
			{
				return NotFound();
			}
			return Ok(user.Projects.ToArray().Select(e => new ProjectDTOUserTime(e.Project, id)));
		}

		// PUT api/Project/5
		public IHttpActionResult PutProject(int id, Project project)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			if (id != project.ID)
			{
				return BadRequest();
			}

			db.Entry(project).State = EntityState.Modified;

			try
			{
				db.SaveChanges();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!ProjectExists(id))
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

		// POST api/Project
		[ResponseType(typeof(ProjectDTO))]
		public IHttpActionResult PostProject(Project project)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			db.Projects.Add(project);
			db.SaveChanges();

			return CreatedAtRoute("DefaultApi", new { id = project.ID }, project);
		}

		// DELETE api/Project/5
		[ResponseType(typeof(ProjectDTO))]
		public IHttpActionResult DeleteProject(int id)
		{
			Project project = db.Projects.Find(id);
			if (project == null)
			{
				return NotFound();
			}

			db.Projects.Remove(project);
			db.SaveChanges();

			return Ok(new ProjectDTO(project));
		}

        private bool ProjectExists(int id)
        {
            return db.Projects.Count(e => e.ID == id) > 0;
        }
    }
}
