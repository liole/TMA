using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeManagementApp.Models
{
	public enum AuthType
	{
		Company, User
	}
	public class AuthData
	{
		public int ID { get; set; }
		public string Login { get; set; }
		public string Password { get; set; }
		public AuthType Type { get; set; }
	}
}