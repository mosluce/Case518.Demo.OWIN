namespace Case518Sample150411.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class SampleDbContext : DbContext
    {
        public SampleDbContext()
            : base("name=SampleDbContext")
        {
        }

        public DbSet<AppUser> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
