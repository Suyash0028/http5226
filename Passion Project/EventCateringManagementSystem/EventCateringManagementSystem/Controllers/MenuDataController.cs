using EventCateringManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using EventCateringManagementSystem.Models.ViewModel;

namespace MenuCateringManagementSystem.Controllers
{
    public class MenuDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all the Menus from the database
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Menus in the database, including their associated menus
        /// </returns>
        /// <example>
        /// GET: api/MenuData/ListMenus
        /// </example>
        [HttpGet]
        [Route("api/MenuData/ListMenus")]
        [ResponseType(typeof(MenuDto))]
        public List<MenuDto> ListMenus()
        {
            List<Menu> Menus = db.Menus.ToList();
            List<MenuDto> MenuDtos = new List<MenuDto>();

            Menus.ForEach(e => MenuDtos.Add(new MenuDto()
            {
                MenuID = e.MenuID,
                MenuTitle = e.MenuTitle,
                MenuDescription = e.MenuDescription,
                EventId = e.EventId,
                EventName = e.Event.EventName,
                EventDescription = e.Event.EventDescription,
                EventLocation = e.Event.EventLocation,
                EventDate = e.Event.EventDate, 
            }));
            return MenuDtos;
        }

        /// <summary>
        /// Gathers information about all events related to a particular Event ID
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all menus in the database, including their associated menus matched with a particular Event ID
        /// </returns>
        /// <param name="id">Event ID.</param>
        /// <example>
        /// GET: api/MenuData/ListMenusForEvents/3
        /// </example>
        [HttpGet]
        [ResponseType(typeof(MenuDto))]
        [Route("api/MenuData/ListMenusForEvents/{id}")]
        public List<MenuDto> ListMenusForEvents(int id)
        {

            List<Menu> Menu = db.Menus.Where(a=>a.EventId == id).ToList();
            List<MenuDto> MenuDtos = new List<MenuDto>();

            Menu.ForEach(m => MenuDtos.Add(new MenuDto()
            {
                MenuID = m.MenuID,
                MenuTitle = m.MenuTitle,
                MenuDescription = m.MenuDescription,
                Menus = m.Menus?.Where(mi=>mi.MenuID == m.MenuID).ToList(),
            })); 

            return MenuDtos;
        }

        /// <summary>
        /// Gathers information about all events related to a particular Event ID
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all menus in the database, not including into that event matched with a particular Event ID
        /// </returns>
        /// <param name="id">Event ID.</param>
        /// <example>
        /// GET: api/MenuData/ListAvailableMenusForEvents/3
        /// </example>
        [HttpGet]
        [ResponseType(typeof(MenuDto))]
        [Route("api/MenuData/ListAvailableMenusForEvents/{id}")]
        public List<MenuDto> ListAvailableMenusForEvents(int id)
        {

            List<Menu> Menu = db.Menus.Where(a => a.EventId != id).ToList();
            List<MenuDto> MenuDtos = new List<MenuDto>();

            Menu.ForEach(m => MenuDtos.Add(new MenuDto()
            {
                MenuID = m.MenuID,
                MenuTitle = m.MenuTitle,
                MenuDescription = m.MenuDescription,
            }));

            return MenuDtos;
        }


        /// <summary>
        /// Returns all Menus in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: An Menu in the system matching up to the MenuID primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of the Menu model</param>
        /// <example>
        /// GET: api/MenuData/FindMenu/5
        /// </example>
        [HttpGet]
        [ResponseType(typeof(MenuDto))]
        [Route("api/MenuData/FindMenu/{id}")]
        public IHttpActionResult FindMenu(int id)
        {
            Menu Menu = db.Menus.Find(id);
            MenuDto MenuDto = new MenuDto()
            {
                MenuID = Menu.MenuID,
                MenuTitle = Menu.MenuTitle,
                MenuDescription = Menu.MenuDescription,
                EventId = Menu.EventId,
                EventName = Menu.Event.EventName,
                EventDescription = Menu.Event.EventDescription,
                EventLocation = Menu.Event.EventLocation,
                EventDate = Menu.Event.EventDate,
            };

            if (Menu == null)
            {
                return NotFound();
            }

            return Ok(MenuDto);
        }


        /// <summary>
        /// Updates a particular Menu in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the Menu ID primary key</param>
        /// <param name="MenuColl">JSON FORM DATA of an Menus</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/MenuData/UpdateMenu/5
        /// FORM DATA: Menu JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        [Route("api/MenuData/UpdateMenu/{id}")]
        public IHttpActionResult UpdateMenu(int id, Menu MenuColl)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != MenuColl.MenuID)
            {

                return BadRequest();
            }

            db.Entry(MenuColl).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MenuExists(id))
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
        /// Adds an Menu to the system
        /// </summary>
        /// <param name="Menu">JSON FORM DATA of an Menu</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: Menu ID, Menu Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/MenuData/AddMenu
        /// FORM DATA: Menu JSON Object
        /// </example>
        [ResponseType(typeof(Menu))]
        [HttpPost]
        [Route("api/MenuData/AddMenu")]

        public IHttpActionResult AddMenu(Menu Menu)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Menus.Add(Menu);
            db.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Deletes an Menu from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the Menu</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/MenuData/DeleteMenu/5
        /// FORM DATA: (empty)
        /// </example>
        [ResponseType(typeof(Menu))]
        [HttpPost]
        [Route("api/MenuData/DeleteMenu/{id}")]

        public IHttpActionResult DeleteMenu(int id)
        {
            Menu Menu = db.Menus.Find(id);
            if (Menu == null)
            {
                return NotFound();
            }

            db.Menus.Remove(Menu);
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

        private bool MenuExists(int id)
        {
            return db.Menus.Count(e => e.MenuID == id) > 0;
        }
    }
}
