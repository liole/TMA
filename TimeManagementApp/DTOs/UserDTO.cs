using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeManagementApp.Models;

namespace TimeManagementApp.DTOs
{
	public class UserDTO
	{
		public int ID { get; set; }
		public string Login { get; set; }
		public string FullName { get; set; }
		public int CompanyID { get; set; }

		public int ProjectsCount { get; set; }

		public UserDTO(User source)
		{
			ID = source.ID;
			Login = source.AuthData.Login;
			FullName = source.FullName;
			CompanyID = source.CompanyID;
			ProjectsCount = source.Projects.Count;
		}
	}
}