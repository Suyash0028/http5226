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

namespace n01629153_User_Management.Controllers
{
    public class UserDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all the Users from the database
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Users in the database.
        /// </returns>
        /// <example>
        /// GET: api/UserData/ListUsers
        /// </example>
        [HttpGet]
        [Route("api/UserData/ListUsers")]
        [ResponseType(typeof(UserDto))]
        public List<UserDto> ListUsers()
        {
            List<User> Users = db.EventsUsers.ToList();
            List<UserDto> UserDtos = new List<UserDto>();

            Users.ForEach(u => UserDtos.Add(new UserDto()
            {
                 UserId = u.UserId,
                 UserName = u.UserName,
                 Password = u.Password,
                 FirstName = u.FirstName,
                 LastName = u.LastName,
                 Email = u.Email,
                 PhoneNumber = u.PhoneNumber,
            }));

            return UserDtos;
        }

        /// <summary>
        /// Returns all Users in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: An User in the system matching up to the UserId primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of the user model</param>
        /// <example>
        /// GET: api/UserData/FindUser/5
        /// </example>
        [HttpGet]
        [ResponseType(typeof(UserDto))]
        [Route("api/UserData/FindUser/{id}")]
        public IHttpActionResult FindUser(int id)
        {
            User User = db.EventsUsers.Find(id);
            UserDto UserDto = new UserDto()
            {
                UserId = User.UserId,
                UserName = User.UserName,
                Password = User.Password,
                FirstName = User.FirstName,
                LastName = User.LastName,
                Email = User.Email,
                PhoneNumber = User.PhoneNumber,
            };

            if (User == null)
            {
                return NotFound();
            }

            return Ok(UserDto);
        }

        /// <summary>
        /// Updates a particular user in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the user ID primary key</param>
        /// <param name="UserColl">JSON FORM DATA of an Users</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/UserData/UpdateUser/5
        /// FORM DATA: User JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        [Route("api/UserData/UpdateUser/{id}")]
        public IHttpActionResult UpdateUser(int id, User UserColl)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != UserColl.UserId)
            {

                return BadRequest();
            }

            db.Entry(UserColl).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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
        /// Adds an User to the system
        /// </summary>
        /// <param name="User">JSON FORM DATA of an User</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: User ID, User Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/UserData/AddUser
        /// FORM DATA: User JSON Object
        /// </example>
        [ResponseType(typeof(User))]
        [HttpPost]
        [Route("api/UserData/AddUser")]
        public IHttpActionResult AddUser(User User)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.EventsUsers.Add(User);
            db.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Deletes an User from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the User</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/UserData/DeleteUser/5
        /// FORM DATA: (empty)
        /// </example>
        [ResponseType(typeof(User))]
        [HttpPost]
        [Route("api/UserData/DeleteUser/{id}")]
        public IHttpActionResult DeleteUser(int id)
        {
            User User = db.EventsUsers.Find(id);
            if (User == null)
            {
                return NotFound();
            }

            db.EventsUsers.Remove(User);
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

        private bool UserExists(int id)
        {
            return db.EventsUsers.Count(e => e.UserId == id) > 0;
        }
    }
}
