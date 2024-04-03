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

namespace FoodCateringManagementSystem.Controllers
{
    public class FoodDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all the Foods from the database
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Foods in the database, including their associated menus
        /// </returns>
        /// <example>
        /// GET: api/FoodData/ListFoods
        /// </example>
        [HttpGet]
        [Route("api/FoodData/ListFoods")]
        [ResponseType(typeof(FoodDto))]
        public List<FoodDto> ListFoods()
        {
            List<Food> Foods = db.Foods.ToList();
            List<FoodDto> FoodDtos = new List<FoodDto>();

            Foods.ForEach(e => FoodDtos.Add(new FoodDto()
            {
                FoodID = e.FoodID,
                FoodName = e.FoodName,
                FoodDescription = e.FoodDescription,
                FoodPrice = e.FoodPrice
            }));
            return FoodDtos;
        }

        /// <summary>
        /// Returns all Foods in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: An Food in the system matching up to the FoodID primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of the Food model</param>
        /// <example>
        /// GET: api/FoodData/FindFood/5
        /// </example>
        [HttpGet]
        [ResponseType(typeof(FoodDto))]
        [Route("api/FoodData/FindFood/{id}")]
        public IHttpActionResult FindFood(int id)
        {
            Food Food = db.Foods.Find(id);
            FoodDto FoodDto = new FoodDto()
            {
                FoodID = Food.FoodID,
                FoodName = Food.FoodName,
                FoodDescription = Food.FoodDescription,
                FoodPrice = Food.FoodPrice
            };

            if (Food == null)
            {
                return NotFound();
            }

            return Ok(FoodDto);
        }


        /// <summary>
        /// Updates a particular Food in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the Food ID primary key</param>
        /// <param name="FoodColl">JSON FORM DATA of an Foods</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/FoodData/UpdateFood/5
        /// FORM DATA: Food JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        [Route("api/FoodData/UpdateFood/{id}")]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult UpdateFood(int id, Food FoodColl)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != FoodColl.FoodID)
            {

                return BadRequest();
            }

            db.Entry(FoodColl).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FoodExists(id))
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
        /// Adds an Food to the system
        /// </summary>
        /// <param name="Food">JSON FORM DATA of an Food</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: Food ID, Food Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/FoodData/AddFood
        /// FORM DATA: Food JSON Object
        /// </example>
        [ResponseType(typeof(Food))]
        [HttpPost]
        [Route("api/FoodData/AddFood")]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult AddFood(Food Food)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Foods.Add(Food);
            db.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Deletes an Food from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the Food</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/FoodData/DeleteFood/5
        /// FORM DATA: (empty)
        /// </example>
        [ResponseType(typeof(Food))]
        [HttpPost]
        [Route("api/FoodData/DeleteFood/{id}")]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult DeleteFood(int id)
        {
            Food Food = db.Foods.Find(id);
            if (Food == null)
            {
                return NotFound();
            }

            db.Foods.Remove(Food);
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

        private bool FoodExists(int id)
        {
            return db.Foods.Count(e => e.FoodID == id) > 0;
        }
    }
}
