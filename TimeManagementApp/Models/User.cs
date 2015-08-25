using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeManagementApp.Models
{
    public class User
    {
        public int ID { get; set; }
        public int AuthDataID { get; set; }
		public string FullName { get; set; }
		public int CompanyID { get; set; }

		public virtual AuthData AuthData { get; set; }
		public virtual Company Company { get; set; }
		public virtual ICollection<JoinUserProject> Projects { get; set; }
		public virtual ICollection<Record> Records { get; set; }
    }
}