using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeManagementApp.Models;

namespace TimeManagementApp.DTOs
{
	public class RecordDTOUser : RecordDTO
	{
		public UserDTO User { get; set; }

		public RecordDTOUser(Record source) :
			base(source)
		{
			this.User = new UserDTO(source.User);
		}
	}
}