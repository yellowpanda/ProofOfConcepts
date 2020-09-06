using NodaTime;
using NodaTime.Extensions;
using NodaTime.TimeZones;
using System;
using System.Linq;
using Xunit;

namespace EntityFrameworkNodaTime
{
    // Presentation on NodaTime https://www.youtube.com/watch?v=JIlO_EebEQI
    // Presentation on NodaTime and Entity Framework https://www.youtube.com/watch?v=zl0h2J6a0w4
    // A nuget package that wraps everything https://www.nuget.org/packages/EntityFrameworkCore.SqlServer.NodaTime/

    public class Test
    {
        [Fact]
        public void CanAdd()
        {
            var databaseContext = new DatabaseContext();
            databaseContext.Database.EnsureDeleted();
            databaseContext.Database.EnsureCreated();
            Instant now = SystemClock.Instance.GetCurrentInstant();
            System.Collections.Generic.IEnumerable<DateTimeZone> timeZoneList = DateTimeZoneProviders.Bcl.GetAllZones().ToList();
            DateTimeZone russianTimeZone = timeZoneList.Single(x=>x.Id == "Russian Standard Time");
            var customer = new Customer()
            {
                Name = "Joe",
                Created = now,
                BirthDay = new LocalDate(2020, 10, 20),
                ZonedDateTime = new ZonedDateTime(SystemClock.Instance.GetCurrentInstant(), russianTimeZone),
            };

            databaseContext.Add(customer);
            databaseContext.SaveChanges();

            databaseContext = new DatabaseContext();
            Customer customerFromDatabase = databaseContext.Customers.Single(x => x.BirthDay == new LocalDate(2020, 10, 20)); 
            Assert.Equal(customer.Created, customerFromDatabase.Created);
            Assert.Equal(customer.Name, customerFromDatabase.Name);

            // The following is not good. The problem is that SQL server do not have a datatype that contains the timezone. 
            Assert.NotEqual(russianTimeZone, customerFromDatabase.ZonedDateTime.Zone);

            databaseContext.Dispose();
        }
    }
}
