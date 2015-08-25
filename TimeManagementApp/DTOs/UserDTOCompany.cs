using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeManagementApp.Models;

namespace TimeManagementApp.DTOs
{
	public class UserDTOCompany : UserDTO
	{
		public CompanyDTO Company { get; set; }

		public UserDTOCompany(User source) :
			base(source)
		{
			Company = new CompanyDTO(source.Company);
		}
	}
}