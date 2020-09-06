using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using NodaTime;

namespace AspNodaTime.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        static readonly List<Event> events = new List<Event>()
        {
            new Event("Concert", new OffsetDateTime(new LocalDateTime(2020, 10, 20, 20, 0), Offset.FromHours(2))),
            new Event("New Year", new OffsetDateTime(new LocalDateTime(2021, 1, 1, 0, 0), Offset.FromHours(1)))
        };

        /// <summary>
        /// Remember to url encode the after parameter: 
        /// ?after=2020-10-20T23:00:00+02:00 should be encoded as ?after=2020-10-20T23:00:00%2B02:00
        /// </summary>
        [HttpGet]
        public IEnumerable<Event> Get(OffsetDateTime after)
        {
            return events.Where(@event => after == null || @event.EventStartTime.ToInstant() > after.ToInstant());
        }



        [HttpPost]
        public void Post([FromBody] Event @event)
        {
            events.Add(@event);
        }

    }
}
