using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeManagementApp.Models;

namespace TimeManagementApp.DTOs
{
	public class ProjectDTO
	{
		public int ID { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }

		public int ParticipantsCount { get; set; }

		public ProjectDTO(Project source)
		{
			ID = source.ID;
			Name = source.Name;
			Description = source.Description;
			ParticipantsCount = source.Participants.Count;
		}
	}
}