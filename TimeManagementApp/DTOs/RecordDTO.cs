using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeManagementApp.Models;

namespace TimeManagementApp.DTOs
{
	public class RecordDTO
	{
		public int ID { get; set; }
		public DateTime BeginTime { get; set; }
		public DateTime EndTime { get; set; }
		public string Description { get; set; }
		public RecordStatus Status { get; set; }
		public int UserID { get; set; }
		public int ProjectID { get; set; }
		public RecordTagDTO Tag { get; set; }

		public RecordDTO(Record source)
		{
			this.ID = source.ID;
			this.BeginTime = source.BeginTime;
			this.EndTime = source.EndTime;
			this.Description = source.Description;
			this.Status = source.Status;
			this.UserID = source.UserID;
			this.ProjectID = source.ProjectID;
			this.Tag = new RecordTagDTO(source.Tag);
		}
	}
}