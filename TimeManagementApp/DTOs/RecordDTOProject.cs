using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeManagementApp.Models;

namespace TimeManagementApp.DTOs
{
	public class RecordDTOProject : RecordDTO
	{
		public ProjectDTO Project { get; set; }

		public RecordDTOProject(Record source) :
			base(source)
		{
			this.Project = new ProjectDTO(source.Project);
		}
	}
}