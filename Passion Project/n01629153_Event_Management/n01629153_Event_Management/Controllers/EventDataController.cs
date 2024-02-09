using n01629153_Event_Management.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Diagnostics;

namespace n01629153_Event_Management.Controllers
{
    public class EventDataController : ApiController
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all the events from the database
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all events in the database, including their associated users and sponsors.
        /// </returns>
        /// <example>
        /// GET: api/EVentData/ListEvents
        /// </example>
        [HttpGet]
        [Route("api/EventData/ListEvents")]
        [ResponseType(typeof(EventDto))]
        public List<EventDto> ListEvents()
        {
            List<Event> Events = db.Events.ToList();
            List<EventDto> EventDtos = new List<EventDto>();

            Events.ForEach(e => EventDtos.Add(new EventDto()
            {
                EventId = e.EventId,
                EventName = e.EventName,
                EventDescription = e.EventDescription,
                EventDate = e.EventDate,
                EventLocation = e.EventLocation,
                SponsorId = e.Sponsors.SponsorId,
                SponsorName = e.Sponsors.SponsorName,
                UserId = e.EventUsers.UserId,
                UserName = e.EventUsers.UserName,
                FirstName = e.EventUsers.FirstName,
                LastName = e.EventUsers.LastName
            }));

            return EventDtos;
        }



        /// <summary>
        /// Gathers information about all events related to a particular sponsor ID
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all events in the database, including their associated sponsor matched with a particular sponsor ID
        /// </returns>
        /// <param name="id">Sponsor ID.</param>
        /// <example>
        /// GET: api/EventData/ListEventsForSponsors/3
        /// </example>
        [HttpGet]
        [ResponseType(typeof(EventDto))]
        [Route("api/EventData/ListEventsForSponsors/{id}")]
        public List<EventDto> ListEventsForSponsors(int id)
        {
            List<Event> Events = db.Events.Where(a => a.SponsorId == id).ToList();
            List<EventDto> EventDtos = new List<EventDto>();

            Events.ForEach(e => EventDtos.Add(new EventDto()
            {
                EventId = e.EventId,
                EventName = e.EventName,
                EventDescription = e.EventDescription,
                EventDate = e.EventDate,
                EventLocation = e.EventLocation,
                SponsorId = e.Sponsors.SponsorId,
                SponsorName = e.Sponsors.SponsorName,
                UserId = e.EventUsers.UserId,
                UserName = e.EventUsers.UserName,
                FirstName = e.EventUsers.FirstName,
                LastName = e.EventUsers.LastName
            }));

            return EventDtos;
        }
        /// <summary>
        /// Gathers information about all events related to a particular user ID
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all events in the database, including their associated user matched with a particular user ID
        /// </returns>
        /// <param name="id">user ID.</param>
        /// <example>
        /// GET: api/EventData/ListEventsForUser/3
        /// </example>
        [HttpGet]
        [ResponseType(typeof(EventDto))]
        [Route("api/EventData/ListEventsForUser/{id}")]
        public List<EventDto> ListEventsForUser(int id)
        {
            List<Event> Events = db.Events.Where(a => a.UserId == id).ToList();
            List<EventDto> EventDtos = new List<EventDto>();

            Events.ForEach(e => EventDtos.Add(new EventDto()
            {
                EventId = e.EventId,
                EventName = e.EventName,
                EventDescription = e.EventDescription,
                EventDate = e.EventDate,
                EventLocation = e.EventLocation,
                SponsorId = e.Sponsors.SponsorId,
                SponsorName = e.Sponsors.SponsorName,
                UserId = e.EventUsers.UserId,
                UserName = e.EventUsers.UserName,
                FirstName = e.EventUsers.FirstName,
                LastName = e.EventUsers.LastName
            }));

            return EventDtos;
        }

        /// <summary>
        /// Returns all events in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: An event in the system matching up to the EventId primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of the event model</param>
        /// <example>
        /// GET: api/EventData/FindEvent/5
        /// </example>
        [HttpGet]
        [ResponseType(typeof(EventDto))]
        [Route("api/EventData/FindEvent/{id}")]
        public IHttpActionResult FindEvent(int id)
        {
            Event Event = db.Events.Find(id);
            EventDto EventDto = new EventDto()
            {
                EventId = Event.EventId,
                EventName = Event.EventName,
                EventDescription = Event.EventDescription,
                EventDate = Event.EventDate,
                EventLocation = Event.EventLocation,
                SponsorId = Event.Sponsors.SponsorId,
                SponsorName = Event.Sponsors.SponsorName,
                UserId = Event.EventUsers.UserId,
                UserName = Event.EventUsers.UserName,
                FirstName = Event.EventUsers.FirstName,
                LastName = Event.EventUsers.LastName
            };

            if (Event == null)
            {
                return NotFound();
            }

            return Ok(EventDto);
        }

        /// <summary>
        /// Updates a particular event in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the event ID primary key</param>
        /// <param name="eventColl">JSON FORM DATA of an events</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/EventData/UpdateEvent/5
        /// FORM DATA: Event JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        [Route("api/EventData/UpdateEvent/{id}")]
        public IHttpActionResult UpdateEvent(int id, Event eventColl)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != eventColl.EventId)
            {

                return BadRequest();
            }

            db.Entry(eventColl).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Adds an event to the system
        /// </summary>
        /// <param name="event">JSON FORM DATA of an event</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: Event ID, Event Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/EventData/AddEvent
        /// FORM DATA: Event JSON Object
        /// </example>
        [ResponseType(typeof(Event))]
        [HttpPost]
        [Route("api/EventData/AddEvent")]
        public IHttpActionResult AddEvent(Event Event)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Events.Add(Event);
            db.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Deletes an Event from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the Event</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/EventData/DeleteEvent/5
        /// FORM DATA: (empty)
        /// </example>
        [ResponseType(typeof(Event))]
        [HttpPost]
        [Route("api/EventData/DeleteEvent/{id}")]
        public IHttpActionResult DeleteEvent(int id)
        {
            Event Event = db.Events.Find(id);
            if (Event == null)
            {
                return NotFound();
            }

            db.Events.Remove(Event);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EventExists(int id)
        {
            return db.Events.Count(e => e.EventId == id) > 0;
        }
    }
}
