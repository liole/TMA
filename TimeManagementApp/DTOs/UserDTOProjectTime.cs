using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeManagementApp.Models;

namespace TimeManagementApp.DTOs
{
	public class UserDTOProjectTime : UserDTO
	{
		public double ProjectHours { get; set; }

		public UserDTOProjectTime(User source, int projectID) :
			base(source)
		{
			var time = new TimeSpan(source.Records
				.Where(r => r.ProjectID == projectID)
				.Select(r => (r.EndTime - r.BeginTime))
				.Sum(t => t.Ticks));
			ProjectHours = time.TotalHours;
		}
	}
}