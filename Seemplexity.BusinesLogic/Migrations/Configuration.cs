using Seemplexity.BusinesLogic.Model;

namespace Seemplexity.BusinesLogic.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Seemplexity.BusinesLogic.Model.SeemplexityModel>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Seemplexity.BusinesLogic.Model.SeemplexityModel context)
        {
            //  This method will be called after migrating to the latest version.

            context.Settings.AddOrUpdate(
                s => s.Name,
                new Setting { Name = "BusTourType", Value = "46" },
                new Setting { Name = "BusServiceKey", Value = "10033" },
                new Setting { Name = "Partner_GloriaTur", Value = "28669" },
                new Setting { Name = "Partner_VernonPrim", Value = "28674" },
                new Setting { Name = "Partner_BalatonSlr", Value = "28675" },
                new Setting { Name = "Partner_VisionTravel", Value = "28751" });

            context.AspNetRoles.AddOrUpdate(r => r.Name,new AspNetRole() { Id = Guid.NewGuid().ToString(), Name = "Admin", AspNetUsers = null });
            context.AspNetRoles.AddOrUpdate(r => r.Name,new AspNetRole() { Id = Guid.NewGuid().ToString(), Name = "Partner_GloriaTur", AspNetUsers = null });
            context.AspNetRoles.AddOrUpdate(r => r.Name,new AspNetRole() { Id = Guid.NewGuid().ToString(), Name = "Partner_VernonPrim", AspNetUsers = null });
            context.AspNetRoles.AddOrUpdate(r => r.Name,new AspNetRole() { Id = Guid.NewGuid().ToString(), Name = "Partner_BalatonSlr", AspNetUsers = null });
            context.AspNetRoles.AddOrUpdate(r => r.Name,new AspNetRole() { Id = Guid.NewGuid().ToString(), Name = "Partner_VisionTravel", AspNetUsers = null });
        }
    }
}
