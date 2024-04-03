using EventCateringManagementSystem.Models.ViewModel;
using EventCateringManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace FoodCateringManagementSystem.Controllers
{
    public class FoodController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static FoodController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44377/api/");
        }

        // GET: Food
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Grabs the authentication cookie sent to this controller.
        /// For proper WebAPI authentication, you can send a post request with login credentials to the WebAPI and log the access token from the response. The controller already knows this token, so we're just passing it up the chain.
        /// 
        /// Here is a descriptive article which walks through the process of setting up authorization/authentication directly.
        /// https://docs.microsoft.com/en-us/aspnet/web-api/overview/security/individual-accounts-in-web-api
        /// </summary>
        private void GetApplicationCookie()
        {
            string token = "";
            //HTTP client is set up to be reused, otherwise it will exhaust server resources.
            //This is a bit dangerous because a previously authenticated cookie could be cached for
            //a follow-up request from someone else. Reset cookies in HTTP client before grabbing a new one.
            client.DefaultRequestHeaders.Remove("Cookie");
            if (!User.Identity.IsAuthenticated) return;

            HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies.Get(".AspNet.ApplicationCookie");
            if (cookie != null) token = cookie.Value;

            //collect token as it is submitted to the controller
            //use it to pass along to the WebAPI.
            Debug.WriteLine("Token Submitted is : " + token);
            if (token != "") client.DefaultRequestHeaders.Add("Cookie", ".AspNet.ApplicationCookie=" + token);

            return;
        }


        // GET: Food/List
        public ActionResult List()
        {
            //objective: communicate with our Food data api to retrieve a list of Foods
            //curl https://localhost:44377/api/FoodData/ListFoods

            string url = "FoodData/ListFoods";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<FoodDto> Foods = response.Content.ReadAsAsync<IEnumerable<FoodDto>>().Result;

            return View(Foods);
        }

        // GET: Food/Details/5
        public ActionResult Details(int id)
        {
            //objective: communicate with our Food data api to retrieve one Food
            //curl https://localhost:44377/api/Fooddata/FindFood/{id}

            DetailFood ViewModel = new DetailFood();

            if (User.Identity.IsAuthenticated && User.IsInRole("Admin")) ViewModel.IsAdmin = true;
            else ViewModel.IsAdmin = false;


            string url = "FoodData/FindFood/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            FoodDto selectedFood = response.Content.ReadAsAsync<FoodDto>().Result;


            ViewModel.SelectedFood = selectedFood;

            return View(ViewModel);
        }

        public ActionResult Error()
        {
            return View();
        }

        // GET: Food/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Food/Create
        [HttpPost]
        
        public ActionResult Create(Food Food)
        {
            Debug.WriteLine("the json payload is :");
            //Debug.WriteLine(Food.FoodName);
            //objective: add a new Food into our system using the API
            //curl -H "Content-Type:application/json" -d @Food.json https://localhost:44377/api/Fooddata/addFood 
            string url = "FoodData/AddFood";


            string jsonpayload = jss.Serialize(Food);

            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Food/Edit/5
        
        public ActionResult Edit(int id)
        {
            //grab the Food information

            //objective: communicate with our Food data api to retrieve one Food
            //curl https://localhost:44377/api/Fooddata/FindFood/{id}

            UpdateFood ViewModel = new UpdateFood();
            string url = "FoodData/FindFood/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            FoodDto selectedFood = response.Content.ReadAsAsync<FoodDto>().Result;
            ViewModel.SelectedFood = selectedFood;


            return View(ViewModel);
        }

        // POST: Food/Update/5
        [HttpPost]
        
        public ActionResult Update(int id, Food Food)
        {
            try
            {

                //serialize into JSON
                //Send the request to the API

                string url = "FoodData/UpdateFood/" + id;


                string jsonpayload = jss.Serialize(Food);
                Debug.WriteLine(jsonpayload);

                HttpContent content = new StringContent(jsonpayload);
                content.Headers.ContentType.MediaType = "application/json";

                //POST: api/FoodData/UpdateFood/{id}
                //Header : Content-Type: application/json
                HttpResponseMessage response = client.PostAsync(url, content).Result;

                return RedirectToAction("Details/" + id);
            }
            catch
            {
                return View();
            }
        }


        // GET: Food/Delete/5
        
        public ActionResult DeleteConfirm(int id)
        {
            string url = "FoodData/FindFood/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            FoodDto selectedFood = response.Content.ReadAsAsync<FoodDto>().Result;
            return View(selectedFood);
        }

        // POST: Food/Delete/5
        [HttpPost]
        
        public ActionResult Delete(int id)
        {
            string url = "FoodData/DeleteFood/" + id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
    }
}