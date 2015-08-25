using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeManagementApp.Models
{
    public class Company
    {
        public int ID { get; set; }
        public int AuthDataID { get; set; }
        public string FullName { get; set; }

		public virtual AuthData AuthData { get; set; }
		public virtual ICollection<User> Employees { get; set; }
		public virtual ICollection<Project> Projects { get; set; }
    }
}