using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeManagementApp.Models
{
    public class RecordTag
    {
        public int ID { get; set; }
        public string Text { get; set; }
        public string Color { get; set; }

		public virtual ICollection<Record> Records { get; set; }
    }
}