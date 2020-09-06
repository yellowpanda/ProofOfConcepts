using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Logging;
using NodaTime;
using System;

namespace EntityFrameworkNodaTime
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Data Source=.;Initial Catalog=SampleDatabase;Integrated Security=True;Pooling=False");
            }

            // Get the SQL statements in the debug window. 
            optionsBuilder.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddDebug()));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Value Convertes are nessasary
            var instantValueConverter = new ValueConverter<Instant, DateTimeOffset>(x => x.ToDateTimeOffset(), x => Instant.FromDateTimeOffset(x));
            var localValueConverter = new ValueConverter<LocalDate, DateTime>(x => x.ToDateTimeUnspecified(), x => LocalDate.FromDateTime(x));
            var zonedTimeValueConverter = new ValueConverter<ZonedDateTime, DateTimeOffset>(x => x.ToDateTimeUnspecified(), x => ZonedDateTime.FromDateTimeOffset(x));

            // The following is not good. The problem is that SQL server do not have a datatype that contains the timezone. 
            var offsetDateTimeValueConverter = new ValueConverter<OffsetDateTime, DateTimeOffset>(x => x.ToDateTimeOffset(), x => OffsetDateTime.FromDateTimeOffset(x));

            modelBuilder.Entity<Customer>()
                .Property(x => x.Created)
                .HasConversion(instantValueConverter)
                .HasColumnType("datetimeoffset");

            modelBuilder.Entity<Customer>()
                .Property(x => x.BirthDay)
                .HasConversion(localValueConverter)
                .HasColumnType("date");

            // The following is not good. The problem is that SQL server do not have a datatype that contains the timezone. 
            modelBuilder.Entity<Customer>()
                .Property(x => x.ZonedDateTime)
                .HasConversion(zonedTimeValueConverter)
                .HasColumnType("datetimeoffset");
        }
    }

    
}
