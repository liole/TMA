using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeManagementApp.Models
{
    public enum RecordStatus
    {
        Complete, InProgress, Scheduled
    }
    public class Record
    {
        public int ID { get; set; }
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Description { get; set; }
        public RecordStatus Status { get; set; }
		public int UserID { get; set; }
		public int ProjectID { get; set; }
		public int TagID { get; set; }

		public virtual RecordTag Tag { get; set; }
		public virtual User User { get; set; }
		public virtual Project Project { get; set; }
    }
}