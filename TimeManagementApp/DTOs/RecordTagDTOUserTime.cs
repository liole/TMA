using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeManagementApp.Models;

namespace TimeManagementApp.DTOs
{
	public class RecordTagDTOUserTime : RecordTagDTO
	{
		public double UserHours { get; set; }

		public RecordTagDTOUserTime(RecordTag source, int userID) :
			base(source)
		{
			var time = new TimeSpan(source.Records
				.Where(r => r.UserID == userID)
				.Select(r => (r.EndTime - r.BeginTime))
				.Sum(t => t.Ticks));
			UserHours = time.TotalHours;

		}

		public RecordTagDTOUserTime(RecordTag source, int userID, int projectID) :
			base(source)
		{
			var time = new TimeSpan(source.Records
				.Where(r => r.UserID == userID)
				.Where(r => r.ProjectID == projectID)
				.Select(r => (r.EndTime - r.BeginTime))
				.Sum(t => t.Ticks));
			UserHours = time.TotalHours;

		}
	}
}