using Microsoft.EntityFrameworkCore;
using Scheduling.Infrastructure.Models;

namespace Scheduling.Data.Sql
{
    public class SchedulingDbContext : DbContext
    {
        public SchedulingDbContext() { }
        public SchedulingDbContext(DbContextOptions<SchedulingDbContext> options) 
            : base(options) { }

        public DbSet<TimeSlot> TimeSlot { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<TimeSlot>(timeSlot =>
            {
                timeSlot.Property(p => p.StartTime).IsRequired();
                timeSlot.Property(p => p.EndTime).IsRequired();
                timeSlot.Property(p => p.UserId).IsRequired();
                timeSlot.HasKey(p => p.TimeSlotId);
            });
        }
    }
}
