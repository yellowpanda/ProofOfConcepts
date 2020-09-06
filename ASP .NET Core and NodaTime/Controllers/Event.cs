using NodaTime;

namespace AspNodaTime.Controllers
{
    public class Event
    {
        public Event()
        {

        }

        public Event(string name, OffsetDateTime eventStartTime)
        {
            Name = name;
            EventStartTime = eventStartTime;
        }

        public string Name { get; set; }
        public OffsetDateTime EventStartTime { get; set;  }
    }
}
