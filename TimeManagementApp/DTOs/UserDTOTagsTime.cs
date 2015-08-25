using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeManagementApp.Models;

namespace TimeManagementApp.DTOs
{
	public class UserDTOTagsTime : UserDTO
	{
		public IEnumerable<RecordTagDTOUserTime> Tags { get; set; }
		public double TotalHours { get; set; }

		public UserDTOTagsTime(User source, IEnumerable<RecordTag> tags) :
			base(source)
		{
			Tags = tags.ToArray().Select(t => new RecordTagDTOUserTime(t, source.ID));
			TotalHours = Tags.Sum(t => t.UserHours);
		}
	}
}