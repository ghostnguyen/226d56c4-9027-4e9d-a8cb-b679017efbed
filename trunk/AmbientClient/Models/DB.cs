using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace AmbientClient.Models
{
    public class DB : DbContext
    {
        public DB()
            : base("AmbientClientDB")
        {

        }
        public DbSet<User> Users { get; set; }
    }

    public class DBInit : DropCreateDatabaseIfModelChanges<DB>
    {
        protected override void Seed(DB context)
        {
            context.Database.ExecuteSqlCommand("CREATE UNIQUE INDEX IX_Users_Email ON Users (Email)");
        }


    }

    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
    }
}