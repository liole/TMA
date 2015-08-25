using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Helpers;
using TimeManagementApp.Models;
using TimeManagementApp.DTOs;

namespace TimeManagementApp.Controllers
{
	public class Credentials
	{
		public string Login { get; set; }
		public string Password { get; set; }
	}

	[RoutePrefix("api/Auth")]
    public class AuthController : TimeAppController
    {
		private static int tokenLength = 16;

		[HttpPost]
		[Route("")]
		public IHttpActionResult Auth(Credentials data)
		{
			AuthData authData = db.AuthData.SingleOrDefault(e => e.Login == data.Login);
			if (authData == null)
			{
				return NotFound();//404
			}
			if (!Crypto.VerifyHashedPassword(authData.Password, data.Password))
			{
				return Unauthorized();//401
			}
			string token = AuthToken.generateRandomString(tokenLength);
			db.AuthTokens.Add(new AuthToken
			{
				Token = token,
				AuthDataID = authData.ID
			});
			db.SaveChanges();
			return Ok(token);
		}
		/*
		[HttpPost]
		[Route("Company")]
		public IHttpActionResult AuthCompany(Credentials data)
		{		

			Company company = db.Companies.SingleOrDefault(e => e.Login == data.Login);
			if (company == null)
			{
				return NotFound();//404
			}
			if (!Crypto.VerifyHashedPassword(company.Password, data.Password))
			{
				return Unauthorized();//401
			}
			string token = AuthToken.generateRandomString(tokenLength);
			db.AuthTokens.Add(new AuthToken	{
				Token = token,
				Type = LoginType.Company,
				LoginID = company.ID
			});
			db.SaveChanges();
			return Ok(token);
		}

		[HttpPost]
		[Route("User")]
		public IHttpActionResult AuthUser(Credentials data)
		{
			User user = db.Users.SingleOrDefault(e => e.Login == data.Login);
			if (user == null)
			{
				return NotFound();//404
			}
			if (!Crypto.VerifyHashedPassword(user.Password, data.Password))
			{
				return Unauthorized();//401
			}
			string token = AuthToken.generateRandomString(tokenLength);
			db.AuthTokens.Add(new AuthToken
			{
				Token = token,
				Type = LoginType.User,
				LoginID = user.ID
			});
			db.SaveChanges();
			return Ok(token);
		}
		*/

		[HttpGet]
		[Route("Type/{login}")]
		[ResponseType(typeof(string))]
		public IHttpActionResult GetLoginType(string login)
		{
			AuthData authData = db.AuthData.SingleOrDefault(e => e.Login == login);
			if (authData == null)
			{
				return NotFound();//404
			}
			switch (authData.Type)
			{
				case AuthType.Company:
					return Ok("Company");
				case AuthType.User:
					return Ok("User");
				default:
					return InternalServerError();//500
			}
		}

		[HttpGet]
		[Route("{token:length(16)}")]
		public IHttpActionResult GetTokenInfo(string token)
		{
			AuthToken auth = db.AuthTokens.SingleOrDefault(a => a.Token == token);
			if (auth == null)
			{
				return NotFound();
			}
			if (auth.AuthData.Type == AuthType.Company)
				return Ok(new
				{
					Type = "Company",
					Data = new CompanyDTO(getCompanyByAuthID(auth.AuthData.ID))
				});
			if (auth.AuthData.Type == AuthType.User)
				return Ok(new
				{
					Type = "User",
					Data = new UserDTO(getUserByAuthID(auth.AuthData.ID))
				});
			return NotFound();
		}
		[HttpDelete]
		[Route("{token:length(16)}")]
		public IHttpActionResult SignOut(string token)
		{
			AuthToken auth = db.AuthTokens.SingleOrDefault(a => a.Token == token);
			if (auth == null)
			{
				return NotFound();
			}

			db.AuthTokens.Remove(auth);
			db.SaveChanges();

			return Ok();
		}
    }
}
