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
using Microsoft.AspNet.Identity;
using System.Net.Sockets;

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
        /// Associates a particular Menu with a particular Food
        /// </summary>
        /// <param name="Foodid">The Food ID primary key</param>
        /// <param name="Menuid">The Menu ID primary key</param>
        /// <param name="Qty">The number of Foods</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// or
        /// HEADER: 400 (BAD REQUEST)
        /// </returns>
        /// <example>
        /// POST api/FoodData/AssociateFoodWithMenu/4/3/2
        /// </example>
        [HttpPost]
        [Route("api/FoodData/AssociateFoodWithMenu/{Foodid}/{Menuid}/{Qty}")]
        //[Authorize(Roles = "Admin,Guest")]
        public IHttpActionResult AssociateFoodWithMenu(int Foodid, int Menuid, int Qty)
        {
            //no negative quantity
            if (Qty < 0) return BadRequest();



            //Try to Find the Food
            Food SelectedFood = db.Foods.Find(Foodid);

            //Try to Find the Menu
            Menu SelectedMenu = db.Menus.Find(Menuid);

            //if Food or Menu doesn't exist return 404
            if (SelectedFood == null || SelectedMenu == null)
            {
                return NotFound();
            }

            //do not process if the (user is not an admin) and (the Menu does not belong to the user)
            bool isAdmin = User.IsInRole("Admin");
            //Forbidden() isn't a natively implemented status like BadRequest()
            //if (!isAdmin && (SelectedMenu.UserID != User.Identity.GetUserId())) return StatusCode(HttpStatusCode.Forbidden);

            //try to update an already existing association between the Food and Menu
            MenuxFood MenuxFood = db.MenuxFoods.Where(MxF => (MxF.FoodID == Foodid && MxF.MenuID == Menuid)).FirstOrDefault();
            if (MenuxFood != null)
            {
                MenuxFood.Quantity = Qty;
                //assume previous price
            }
            //otherwise add a new association between the Food and the Menu
            else
            {
                //Get the current price of the Food
                decimal FoodPrice = SelectedFood.FoodPrice;

                //Create a new instance of Food x Menu
                MenuxFood NewMxF = new MenuxFood()
                {
                    Food = SelectedFood,
                    Menu = SelectedMenu,
                    Quantity = Qty,
                    Price = FoodPrice
                };
                db.MenuxFoods.Add(NewMxF);
            }
            db.SaveChanges();
            return Ok();
        }

        /// <summary>
        /// Removes an association between a particular Menu and a particular Food
        /// function is deprecated (not in use). Just use a different qty with 'AssociateFoodWithMenu'
        /// </summary>
        /// <param name="MxFID">Menu X Food Primary key</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST api/FoodData/UnAssociateFoodWithMenu/200
        /// </example>
        [HttpPost]
        [Route("api/FoodData/UnAssociateFoodWithMenu/{MxFID}")]
        [Authorize]
        public IHttpActionResult UnAssociateFoodWithMenu(int MxFID)
        {

            //Note: this could also be done with the two FK Food ID and Menu ID
            //find the Menu x Food
            MenuxFood SelectedMxF = db.MenuxFoods.Find(MxFID);
            if (SelectedMxF == null) return NotFound();

            db.MenuxFoods.Remove(SelectedMxF);
            db.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Gathers information about all Foods related to a particular menu ID
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Foods in the database, including their associated menu matched with a particular menu ID
        /// </returns>
        /// <param name="id">Menu ID.</param>
        /// <example>
        /// GET: api/FoodData/ListFoodsForMenu/3
        /// </example>
        [HttpGet]
        [ResponseType(typeof(FoodDto))]
        [Route("api/FoodData/ListFoodsForMenu/{id}")]
        public List<MenuxFoodDto> ListFoodsForMenu(int id)
        {
            List<MenuxFood> Foods = db.MenuxFoods.Where(a => a.MenuID == id).ToList();
            List<MenuxFoodDto> FoodDtos = new List<MenuxFoodDto>();

            foreach (var food in Foods)
            {
                
                Food foodDetails = db.Foods.Find(food.FoodID);

                FoodDtos.Add(new MenuxFoodDto()
                {
                    FoodID = foodDetails.FoodID,
                    FoodName = foodDetails.FoodName,
                    FoodDescription = foodDetails.FoodDescription,
                    FoodPrice = foodDetails.FoodPrice,
                    Quantity = food.Quantity,
                });
            }


            return FoodDtos;
        }

        /// <summary>
        /// Gathers information about all Foods related to a particular menu ID
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Foods in the database, including their associated menu matched with a particular menu ID
        /// </returns>
        /// <param name="id">Menu ID.</param>
        /// <example>
        /// GET: api/FoodData/ListAvailableFoodsForMenu/3
        /// </example>
        [HttpGet]
        [ResponseType(typeof(FoodDto))]
        [Route("api/FoodData/ListAvailableFoodsForMenu/{id}")]
        public List<MenuxFoodDto> ListAvailableFoodsForMenu(int id)
        {
            List<MenuxFood> Foods = db.MenuxFoods.Where(a => a.MenuID != id).ToList();
            List<MenuxFoodDto> FoodDtos = null;

            foreach (var food in Foods)
            {

                Food foodDetails = db.Foods.Find(food.FoodID);

                FoodDtos.Add(new MenuxFoodDto()
                {
                    FoodID = foodDetails.FoodID,
                    FoodName = foodDetails.FoodName,
                    FoodDescription = foodDetails.FoodDescription,
                    FoodPrice = foodDetails.FoodPrice,
                    Quantity = food.Quantity,
                });
            }


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
