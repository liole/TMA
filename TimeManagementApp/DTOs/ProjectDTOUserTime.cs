using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeManagementApp.Models;

namespace TimeManagementApp.DTOs
{
	public class ProjectDTOUserTime : ProjectDTO
	{
		public double UserHours { get; set; }

		public ProjectDTOUserTime(Project source, int userID) :
			base(source)
		{
			var time = new TimeSpan(source.Records
				.Where(r => r.UserID == userID)
				.Select(r => (r.EndTime - r.BeginTime))
				.Sum(t => t.Ticks));
			UserHours = time.TotalHours;

		}
	}
}