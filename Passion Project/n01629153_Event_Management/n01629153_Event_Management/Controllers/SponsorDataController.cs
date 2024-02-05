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

namespace n01629153_Event_Management.Controllers
{
    public class SponsorDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        /// <summary>
        /// Returns all the sponsors from the database
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Sponsors in the database.
        /// </returns>
        /// <example>
        /// GET: api/SponsorData/ListSponsors
        /// </example>
        [HttpGet]
        [Route("api/SponsorData/ListSponsors")]
        [ResponseType(typeof(SponsorDto))]
        public List<SponsorDto> ListSponsors()
        {
            List<Sponsor> Sponsors = db.Sponsors.ToList();
            List<SponsorDto> SponsorDtos = new List<SponsorDto>();

            Sponsors.ForEach(s => SponsorDtos.Add(new SponsorDto()
            {
               SponsorId = s.SponsorId,
               SponsorName = s.SponsorName,
               ContactEmail = s.ContactEmail,
               ContactPerson = s.ContactPerson,
            }));

            return SponsorDtos;
        }

        /// <summary>
        /// Returns all Sponsors in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: An Sponsor in the system matching up to the EventId primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of the Sponsor model</param>
        /// <example>
        /// GET: api/SponsorData/FindSponsor/5
        /// </example>
        [HttpGet]
        [ResponseType(typeof(SponsorDto))]
        [Route("api/SponsorData/FindSponsor/{id}")]
        public IHttpActionResult FindSponsor(int id)
        {
            Sponsor Sponsor = db.Sponsors.Find(id);
            SponsorDto SponsorDto = new SponsorDto()
            {
                SponsorId = Sponsor.SponsorId,
                SponsorName = Sponsor.SponsorName,
                ContactEmail = Sponsor.ContactEmail,
                ContactPerson = Sponsor.ContactPerson,
            };

            if (Sponsor == null)
            {
                return NotFound();
            }

            return Ok(SponsorDto);
        }

        /// <summary>
        /// Updates a particular Sponsor in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the Sponsor ID primary key</param>
        /// <param name="sponsors">JSON FORM DATA of an Sponsors</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/SponsorData/UpdateSponsor/5
        /// FORM DATA: Sponsor JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        [Route("api/SponsorData/UpdateSponsor/{id}")]
        public IHttpActionResult UpdateSponsor(int id, Sponsor sponsors)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != sponsors.SponsorId)
            {

                return BadRequest();
            }

            db.Entry(sponsors).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SponsorExists(id))
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
        /// Adds an Sponsor to the system
        /// </summary>
        /// <param name="Sponsor">JSON FORM DATA of an Sponsor</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: Sponsor ID, Sponsor Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/SponsorData/AddSponsor
        /// FORM DATA: Sponsor JSON Object
        /// </example>
        [ResponseType(typeof(Sponsor))]
        [HttpPost]
        [Route("api/SponsorData/AddSponsor")]
        public IHttpActionResult AddSponsor(Sponsor Sponsor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Sponsors.Add(Sponsor);
            db.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Deletes an Sponsor from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the Sponsor</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/SponsorData/DeleteSponsor/5
        /// FORM DATA: (empty)
        /// </example>
        [ResponseType(typeof(Sponsor))]
        [HttpPost]
        [Route("api/SponsorData/DeleteSponsor/{id}")]
        public IHttpActionResult DeleteSponsor(int id)
        {
            Sponsor Sponsor = db.Sponsors.Find(id);
            if (Sponsor == null)
            {
                return NotFound();
            }

            db.Sponsors.Remove(Sponsor);
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

        private bool SponsorExists(int id)
        {
            return db.Sponsors.Count(e => e.SponsorId == id) > 0;
        }
    }
}
