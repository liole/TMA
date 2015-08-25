using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeManagementApp.Models
{
	public class JoinUserProject
	{
		public int ID { get; set; }
		public int UserID { get; set; }
		public int ProjectID { get; set; }

		public virtual User User { get; set; }
		public virtual Project Project { get; set; }
	}
}