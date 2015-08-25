using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using TimeManagementApp.Models;
using System.Web.Helpers;

namespace TimeManagementApp.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<DataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DataContext context)
        {
			context.AuthData.AddOrUpdate(
				new Models.AuthData {
					ID = 1,
					Login = "LNU",
					Password = Crypto.HashPassword("password"),
					Type = AuthType.Company
				},
				new Models.AuthData
				{
					ID = 2,
					Login = "InterLogic",
					Password = Crypto.HashPassword("pas123"),
					Type = AuthType.Company
				},
				new Models.AuthData
				{
					ID = 3,
					Login = "Oleg",
					Password = Crypto.HashPassword("skleroz"),
					Type = AuthType.User
				},
				new Models.AuthData
				{
					ID = 4,
					Login = "User1",
					Password = Crypto.HashPassword("pas"),
					Type = AuthType.User
				}
			);
			context.Companies.AddOrUpdate(
				new Company {
					ID = 1,
					AuthDataID = 1,
					FullName = "Ћьв≥вський нац≥ональний ун≥верситет ≥м. ≤.‘ранка"
				},
				new Company {
					ID = 2,
					AuthDataID = 2,
					FullName = "“зќ¬ \"≤нтерЋог≥ка\""
				}
			);
			context.Users.AddOrUpdate(
				new User {
					ID = 1,
					AuthDataID = 3,
					FullName = "ќлег ѕилипчак",
					CompanyID = 1
				},
				new User {
					ID = 2,
					AuthDataID = 4,
					FullName = "ƒругий користувач",
					CompanyID = 1
				}
			);
			context.Projects.AddOrUpdate(
				new Project {
					ID = 1,
					Name = "Time Management App",
					Description = "Web application for time management",
					CompanyID = 1
				}
			);
			context.JoinsUserProject.AddOrUpdate(
				new JoinUserProject { 
					ID = 1,
					ProjectID = 1,
					UserID = 1
				}
			);
			context.RecordTags.AddOrUpdate(
				new RecordTag { 
					ID = 1,
					Text = "reading",
					Color = "#ddeeff"
				},
				new RecordTag { 
					ID = 2,
					Text = "coding",
					Color = "#eeffdd"
				},
				new RecordTag
				{
					ID = 3,
					Text = "designing",
					Color = "#ffffcc"
				}
			);
			context.Records.AddOrUpdate(
				new Record { 
					ID = 1,
					BeginTime = new DateTime(2015, 6, 13, 11, 0, 0),
					EndTime = new DateTime(2015, 6, 13, 13, 0, 0),
					Description = "Reading about WebAPI and AngularJS single-page applications",
					Status = RecordStatus.Complete,
					TagID = 1,
					UserID = 1,
					ProjectID = 1
				},
				new Record
				{
					ID = 2,
					BeginTime = new DateTime(2015, 6, 13, 15, 0, 0),
					EndTime = new DateTime(2015, 6, 13, 20, 0, 0),
					Description = "Creating models, Company controller and initial data configuration",
					Status = RecordStatus.Complete,
					TagID = 2,
					UserID = 1,
					ProjectID = 1
				},
				new Record
				{
					ID = 3,
					BeginTime = new DateTime(2015, 6, 15, 14, 0, 0),
					EndTime = new DateTime(2015, 6, 15, 20, 0, 0),
					Description = "Creating templete, designing login page",
					Status = RecordStatus.Complete,
					TagID = 3,
					UserID = 1,
					ProjectID = 1
				}
			);
        }
    }
}
