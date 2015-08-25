using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeManagementApp.Models;

namespace TimeManagementApp.DTOs
{
	public class RecordTagDTO
	{
		public int ID { get; set; }
		public string Text { get; set; }
		public string Color { get; set; }

		public RecordTagDTO(RecordTag source)
		{
			this.ID = source.ID;
			this.Text = source.Text;
			this.Color = source.Color;
		}
	}
}