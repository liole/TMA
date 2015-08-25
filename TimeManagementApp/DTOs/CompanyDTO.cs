using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeManagementApp.Models;

namespace TimeManagementApp.DTOs
{
	public class CompanyDTO
	{
		public int ID { get; set; }
		public string Login { get; set; }
		public string FullName { get; set; }

		public int EmployeesCount { get; set; }
		public int ProjectsCount { get; set; }

		public CompanyDTO(Company source)
		{
			ID = source.ID;
			Login = source.AuthData.Login;
			FullName = source.FullName;
			EmployeesCount = source.Employees.Count;
			ProjectsCount = source.Projects.Count;
		}
	}
}