using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using TimeManagementApp.Models;
using TimeManagementApp.DTOs;

namespace TimeManagementApp.Controllers
{
	[RoutePrefix("api/RecordTag")]
	public class RecordTagController : TimeAppController
	{
		[HttpGet]
		[Route("List")]
		public IEnumerable<RecordTagDTO> GetUsersList()
		{
			return db.RecordTags.ToArray().Select(e => new RecordTagDTO(e));
		}

		[HttpGet]
		[Route("~/api/User/{id}/TagStatistic")]
		[AuthThisUser]
		public IEnumerable<RecordTagDTOUserTime> GetTagStatisticByUser(int id)
		{
			return db.RecordTags.ToArray().Select(e => new RecordTagDTOUserTime(e, id));
		}

		[HttpGet]
		[Route("~/api/User/{user}/Project/{id}/TagStatistic")]
		[AuthThisProjectCompany]
		public IEnumerable<RecordTagDTOUserTime> GetTagStatisticByUserAndProject(int user, int id)
		{
			return db.RecordTags.ToArray().Select(e => new RecordTagDTOUserTime(e, user, id));
		}
    }
}
