using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeManagementApp.Models
{
    public class Project
    {
        public int ID { get; set; }
        public string Name { get; set; }
		public string Description { get; set; }
		public int CompanyID { get; set; }

        public virtual Company Company { get; set; }
        public virtual ICollection<JoinUserProject> Participants { get; set; }
		public virtual ICollection<Record> Records { get; set; }
    }
}