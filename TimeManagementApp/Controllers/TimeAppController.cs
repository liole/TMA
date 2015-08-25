using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TimeManagementApp.Models;
using TimeManagementApp.DTOs;

namespace TimeManagementApp.Controllers
{
    public class TimeAppController : ApiController
    {
		public DataContext db = new DataContext();

		[NonAction]
		public Company getCompanyByAuthID (int authID)
		{
			return db.Companies.SingleOrDefault(e => e.AuthDataID == authID);
		}

		[NonAction]
		public User getUserByAuthID(int authID)
		{
			return db.Users.SingleOrDefault(e => e.AuthDataID == authID);
		}

		[NonAction]
		public bool isAuthorized()
		{
			IEnumerable<string> tokens;
			ControllerContext.Request.Headers.TryGetValues("AuthToken", out tokens);
			return tokens != null;
		}

		[NonAction]
		public AuthToken getCurrentAuthToken()
		{
			IEnumerable<string> tokens;
			ControllerContext.Request.Headers.TryGetValues("AuthToken", out tokens);
			if (tokens == null)
				return null;
			return db.AuthTokens.SingleOrDefault(t => t.Token == tokens.FirstOrDefault());
		}

		[NonAction]
		public AuthData getCurrentAuthData()
		{
			return getCurrentAuthToken().AuthData;
		}

		[NonAction]
		public User getCurrentUser()
		{
			return getUserByAuthID(getCurrentAuthData().ID);
		}

		[NonAction]
		public Company getCurrentCompany()
		{
			return getCompanyByAuthID(getCurrentAuthData().ID);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				db.Dispose();
			}
			base.Dispose(disposing);
		}

    }
}
