using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeManagementApp.Models
{
	public class AuthToken
	{
		public int ID { get; set; }
		public string Token { get; set; }
		public int AuthDataID { get; set; }

		public virtual AuthData AuthData { get; set; }

		public static string generateRandomString(int length)
		{
			var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
			var stringChars = new char[length];
			var random = new Random();
			for (int i = 0; i < stringChars.Length; i++)
				stringChars[i] = chars[random.Next(chars.Length)];
			return new String(stringChars);
		}
	}
}