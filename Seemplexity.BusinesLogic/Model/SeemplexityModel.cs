namespace Seemplexity.BusinesLogic.Model
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class SeemplexityModel : DbContext
    {
        public SeemplexityModel()
            : base("name=Seemplexity")
        {
        }

        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

        public virtual DbSet<Setting> Settings { get; set; }
        //public virtual DbSet<ServiceListToTransport> ServiceListToTransports { get; set; }
        public virtual DbSet<BusDescription> BusDescriptions { get; set; }
        public virtual DbSet<BusDescriptionScheduleDates> BusDescriptionScheduleDates { get; set; }
        public virtual DbSet<BinaryItem> BinaryItems { get; set; }
        public virtual DbSet<BusSchemeFloorDescription> BusSchemeFloorDescriptions { get; set; }
        public virtual DbSet<BusSchemeItem> BusSchemeFloors { get; set; }
        public virtual DbSet<HotelMapping> HotelMappings { get; set; }
        public virtual DbSet<ExcursionMapping> ExcursionMappings { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetRole>()
                .HasMany(e => e.AspNetUsers)
                .WithMany(e => e.AspNetRoles)
                .Map(m => m.ToTable("AspNetUserRoles").MapLeftKey("RoleId").MapRightKey("UserId"));

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.AspNetUserClaims)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.AspNetUserLogins)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<BusSchemeFloorDescription>()
                .HasMany(d => d.Items)
                .WithRequired(i => i.FloorDescription)
                .WillCascadeOnDelete(true);
        }

    }
}
