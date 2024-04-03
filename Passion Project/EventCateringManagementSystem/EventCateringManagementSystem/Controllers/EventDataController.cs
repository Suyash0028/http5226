using EventCateringManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Diagnostics;

namespace EventCateringManagementSystem.Controllers
{
    public class EventDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all the events from the database
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all events in the database, including their associated menus
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
                EventLocation = e.EventLocation
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
                EventLocation = Event.EventLocation
            };

            if (Event == null)
            {
                return NotFound();
            }

            return Ok(EventDto);
        }

        /// <summary>
        /// Associates a particular Menu with a particular Event
        /// </summary>
        /// <param name="Eventid">The Event ID primary key</param>
        /// <param name="Menuid">The Menu ID primary key</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST api/EventData/AssociateEventWithMenu/9/1
        /// </example>
        [HttpPost]
        [Route("api/EventData/AssociateEventWithMenu/{Eventid}/{Menuid}")]
        
        public IHttpActionResult AssociateEventWithMenu(int Eventid, int Menuid)
        {

            Event SelectedEvent = db.Events.Include(a => a.Menus).Where(a => a.EventId == Eventid).FirstOrDefault();
            Menu SelectedMenu = db.Menus.Find(Menuid);

            if (SelectedEvent == null || SelectedMenu == null)
            {
                return NotFound();
            }


            SelectedEvent.Menus.Add(SelectedMenu);
            db.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Removes an association between a particular Menu and a particular Event
        /// </summary>
        /// <param name="Eventid">The Event ID primary key</param>
        /// <param name="Menuid">The Menu ID primary key</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST api/EventData/UnAssociateEventWithMenu/9/1
        /// </example>
        [HttpPost]
        [Route("api/EventData/UnAssociateEventWithMenu/{EventId}/{MenuId}")]
        
        public IHttpActionResult UnAssociateEventWithMenu(int Eventid, int Menuid)
        {

            Event SelectedEvent = db.Events.Include(a => a.Menus).Where(a => a.EventId == Eventid).FirstOrDefault();
            Menu SelectedMenu = db.Menus.Find(Menuid);

            if (SelectedEvent == null || SelectedMenu == null)
            {
                return NotFound();
            }


            SelectedEvent.Menus.Remove(SelectedMenu);
            db.SaveChanges();

            return Ok();
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
