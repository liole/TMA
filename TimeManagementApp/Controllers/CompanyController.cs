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
using TimeManagementApp.Models;
using TimeManagementApp.DTOs;

namespace TimeManagementApp.Controllers
{
	[RoutePrefix("api/Company")]
    public class CompanyController : TimeAppController
    {
		[HttpGet]
        [Route("List")]
        public IEnumerable<CompanyDTO> GetCompaniesList()
        {
            return db.Companies.ToArray().Select(e => new CompanyDTO(e));
        }

        [HttpGet]
		[Route("{id}")]
        [ResponseType(typeof(CompanyDTO))]
        public IHttpActionResult GetCompanySummary(int id)
        {
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return NotFound();
            }
            return Ok(new CompanyDTO(company));
        }

		[HttpGet]
		[Route("Info/{login}")]
		[ResponseType(typeof(CompanyDTO))]
		public IHttpActionResult GetCompanyByLogin(string login)
		{
			Company company = db.Companies.SingleOrDefault(e => e.AuthData.Login == login);
			if (company == null)
			{
				return NotFound();
			}
			return Ok(new CompanyDTO(company));
		}

        // PUT api/Company/5
        public IHttpActionResult PutCompany(int id, Company company)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != company.ID)
            {
                return BadRequest();
            }

            db.Entry(company).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompanyExists(id))
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

        // POST api/Company
        [ResponseType(typeof(CompanyDTO))]
        public IHttpActionResult PostCompany(Company company)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Companies.Add(company);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = company.ID }, company);
        }

        // DELETE api/Company/5
        [ResponseType(typeof(CompanyDTO))]
        public IHttpActionResult DeleteCompany(int id)
        {
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return NotFound();
            }

            db.Companies.Remove(company);
            db.SaveChanges();

            return Ok(new CompanyDTO(company));
        }

        private bool CompanyExists(int id)
        {
            return db.Companies.Count(e => e.ID == id) > 0;
        }
    }
}