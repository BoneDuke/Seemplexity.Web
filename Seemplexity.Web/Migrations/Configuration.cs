using Seemplexity.BusinesLogic.Model;

namespace Seemplexity.Web.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<SeemplexityModel>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(SeemplexityModel context)
        {
            context.Settings.AddOrUpdate(
                s => s.Name,
                new Setting { Name = "BusTourType", Value = "46" },
                new Setting { Name = "BusServiceKey", Value = "10034" });

            context.AspNetRoles.AddOrUpdate(
                r => r.Name,
                new AspNetRole() { Id = Guid.NewGuid().ToString(), Name = "Admin", AspNetUsers = null});

            context.SaveChanges();
        }
    }
}
