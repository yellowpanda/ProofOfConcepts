using NodaTime;

namespace EntityFrameworkNodaTime
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Instant Created { get; set; }
        public LocalDate BirthDay { get; set; }

        public ZonedDateTime ZonedDateTime { get; set; }
    }
}
