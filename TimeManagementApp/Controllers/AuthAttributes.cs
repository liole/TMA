using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Net;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;


namespace TimeManagementApp.Controllers
{
	public class AuthCompany : ActionFilterAttribute
	{
		public override void OnActionExecuting(HttpActionContext actionContext)
		{
			var controller = actionContext.ControllerContext.Controller as TimeAppController;
			if (!controller.isAuthorized())
			{
				actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
				return;
			}
			var token = controller.getCurrentAuthToken();
			if (token == null || token.AuthData.Type != Models.AuthType.Company)
			{
				actionContext.Response = new HttpResponseMessage(HttpStatusCode.Forbidden);
				return;
			}
		}
	}

	public class AuthUser : ActionFilterAttribute
	{
		public override void OnActionExecuting(HttpActionContext actionContext)
		{
			var controller = actionContext.ControllerContext.Controller as TimeAppController;
			if (!controller.isAuthorized())
			{
				actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
				return;
			}
			var token = controller.getCurrentAuthToken();
			if (token == null || token.AuthData.Type != Models.AuthType.User)
			{
				actionContext.Response = new HttpResponseMessage(HttpStatusCode.Forbidden);
				return;
			}
		}
	}

	public class AuthThisCompany : ActionFilterAttribute
	{
		public override void OnActionExecuting(HttpActionContext actionContext)
		{
			var routeData = actionContext.Request.GetRouteData();
			var id = int.Parse(routeData.Values["id"] as string);
			var controller = actionContext.ControllerContext.Controller as TimeAppController;
			if (!controller.isAuthorized()) {
				actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
				return;
			}
			var token = controller.getCurrentAuthToken();
			if (token == null || token.AuthData.Type != Models.AuthType.Company || controller.getCompanyByAuthID(token.AuthDataID).ID != id)
			{
				actionContext.Response = new HttpResponseMessage(HttpStatusCode.Forbidden);
				return;
			}
		}
	}

	public class AuthThisUser : ActionFilterAttribute
	{
		public override void OnActionExecuting(HttpActionContext actionContext)
		{
			var routeData = actionContext.Request.GetRouteData();
			var id = int.Parse(routeData.Values["id"] as string);
			var controller = actionContext.ControllerContext.Controller as TimeAppController;
			if (!controller.isAuthorized())
			{
				actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
				return;
			}
			var token = controller.getCurrentAuthToken();
			if (token == null || token.AuthData.Type != Models.AuthType.User || controller.getUserByAuthID(token.AuthDataID).ID != id)
			{
				actionContext.Response = new HttpResponseMessage(HttpStatusCode.Forbidden);
				return;
			}
		}
	}

	public class AuthThisUserOrHisCompany : ActionFilterAttribute
	{
		public override void OnActionExecuting(HttpActionContext actionContext)
		{
			var routeData = actionContext.Request.GetRouteData();
			var id = int.Parse(routeData.Values["id"] as string);
			var controller = actionContext.ControllerContext.Controller as TimeAppController;
			var user = controller.db.Users.Find(id);
			if (!controller.isAuthorized())
			{
				actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
				return;
			}
			var token = controller.getCurrentAuthToken();
			if (token == null || (
				!(token.AuthData.Type == Models.AuthType.User && controller.getUserByAuthID(token.AuthDataID).ID == id) &&
				!(user != null && token.AuthData.Type == Models.AuthType.Company && controller.getCompanyByAuthID(token.AuthDataID).ID == user.CompanyID)))
			{
				actionContext.Response = new HttpResponseMessage(HttpStatusCode.Forbidden);
				return;
			}
		}
	}

	public class AuthThisUserCompany : ActionFilterAttribute
	{
		public override void OnActionExecuting(HttpActionContext actionContext)
		{
			var routeData = actionContext.Request.GetRouteData();
			var id = int.Parse(routeData.Values["id"] as string);
			var controller = actionContext.ControllerContext.Controller as TimeAppController;
			var comapnyID = controller.db.Users.Find(id).CompanyID;
			if (!controller.isAuthorized())
			{
				actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
				return;
			}
			var token = controller.getCurrentAuthToken();
			if (token == null || token.AuthData.Type != Models.AuthType.Company || controller.getCompanyByAuthID(token.AuthDataID).ID != comapnyID)
			{
				actionContext.Response = new HttpResponseMessage(HttpStatusCode.Forbidden);
				return;
			}
		}
	}

	public class AuthThisProjectCompany : ActionFilterAttribute
	{
		public override void OnActionExecuting(HttpActionContext actionContext)
		{
			var routeData = actionContext.Request.GetRouteData();
			var id = int.Parse(routeData.Values["id"] as string);
			var controller = actionContext.ControllerContext.Controller as TimeAppController;
			var comapnyID = controller.db.Projects.Find(id).CompanyID;
			if (!controller.isAuthorized())
			{
				actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
				return;
			}
			var token = controller.getCurrentAuthToken();
			if (token == null || token.AuthData.Type != Models.AuthType.Company || controller.getCompanyByAuthID(token.AuthDataID).ID != comapnyID)
			{
				actionContext.Response = new HttpResponseMessage(HttpStatusCode.Forbidden);
				return;
			}
		}
	}

	public class AuthThisProjectAssociate : ActionFilterAttribute
	{
		public override void OnActionExecuting(HttpActionContext actionContext)
		{
			var routeData = actionContext.Request.GetRouteData();
			var id = int.Parse(routeData.Values["id"] as string);
			var controller = actionContext.ControllerContext.Controller as TimeAppController;
			var project = controller.db.Projects.Find(id);
			var comapnyID = project.CompanyID;
			if (!controller.isAuthorized())
			{
				actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
				return;
			}
			var token = controller.getCurrentAuthToken();
			if (token == null || 
				(token.AuthData.Type == Models.AuthType.Company 
					&& controller.getCompanyByAuthID(token.AuthDataID).ID != comapnyID) ||
				(token.AuthData.Type == Models.AuthType.User 
					&& controller.getUserByAuthID(token.AuthDataID).Projects.SingleOrDefault(e => e.ProjectID == id) == null))
			{
				actionContext.Response = new HttpResponseMessage(HttpStatusCode.Forbidden);
				return;
			}
		}
	}
}