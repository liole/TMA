using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace TimeManagementApp.Models
{
    public class DataContext : DbContext
    {

		public DbSet<Company> Companies { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<Project> Projects { get; set; }
		public DbSet<JoinUserProject> JoinsUserProject { get; set; }
		public DbSet<Record> Records { get; set; }
		public DbSet<RecordTag> RecordTags { get; set; }
		public DbSet<AuthToken> AuthTokens { get; set; }
		public DbSet<AuthData> AuthData { get; set; }

        public DataContext() : base("name=DataContext")
        {
        }
    
    }
}
