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

namespace MenuCateringManagementSystem.Controllers
{
    public class MenuController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static MenuController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44377/api/");
        }

        // GET: Menu
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


        // GET: Menu/List
        public ActionResult List()
        {
            //objective: communicate with our Menu data api to retrieve a list of Menus
            //curl https://localhost:44377/api/MenuData/ListMenus

            string url = "MenuData/ListMenus";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<MenuDto> Menus = response.Content.ReadAsAsync<IEnumerable<MenuDto>>().Result;

            return View(Menus);
        }

        // GET: Menu/Details/5
        public ActionResult Details(int id)
        {
            //objective: communicate with our Menu data api to retrieve one Menu
            //curl https://localhost:44377/api/Menudata/FindMenu/{id}

            DetailMenu ViewModel = new DetailMenu();

            if (User.Identity.IsAuthenticated && User.IsInRole("Admin")) ViewModel.IsAdmin = true;
            else ViewModel.IsAdmin = false;


            string url = "MenuData/FindMenu/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            MenuDto selectedMenu = response.Content.ReadAsAsync<MenuDto>().Result;


            ViewModel.SelectedMenu = selectedMenu;

            return View(ViewModel);
        }

        //POST: Menu/Associate
        [HttpPost]
        //[Authorize(Roles = "Admin,Guest")]
        public ActionResult Associate(int FoodID, int MenuID, int Qty)
        {
            GetApplicationCookie();//get token credentials


            //call our api to associate Menu with keeper
            string url = "FoodData/AssociateFoodWithMenu/" + FoodID + "/" + MenuID + "/" + Qty;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Details/" + MenuID);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }


        //Get: Menu/UnAssociate/{MxFID}?MenuID={MenuID}
        //Deprecated. Use Associate instead to change quantity
        [HttpGet]
        //[Authorize(Roles = "Admin,Guest")]
        public ActionResult UnAssociate(int id, int MenuID)
        {
            GetApplicationCookie();//get token credentials


            //call our api to remove a Menu x Food
            string url = "FoodData/UnAssociateFoodWithMenu/" + id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("Details/" + MenuID);
        }

        public ActionResult Error()
        {
            return View();
        }

        // GET: Menu/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Menu/Create
        [HttpPost]

        public ActionResult Create(Menu Menu)
        {
            Debug.WriteLine("the json payload is :");
            //Debug.WriteLine(Menu.MenuName);
            //objective: add a new Menu into our system using the API
            //curl -H "Content-Type:application/json" -d @Menu.json https://localhost:44377/api/Menudata/addMenu 
            string url = "MenuData/AddMenu";


            string jsonpayload = jss.Serialize(Menu);

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

        // GET: Menu/Edit/5

        public ActionResult Edit(int id)
        {
            //grab the Menu information

            //objective: communicate with our Menu data api to retrieve one Menu
            //curl https://localhost:44377/api/Menudata/FindMenu/{id}

            UpdateMenu ViewModel = new UpdateMenu();
            string url = "MenuData/FindMenu/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            MenuDto selectedMenu = response.Content.ReadAsAsync<MenuDto>().Result;
            ViewModel.SelectedMenu = selectedMenu;


            return View(ViewModel);
        }

        // POST: Menu/Update/5
        [HttpPost]

        public ActionResult Update(int id, Menu Menu)
        {
            try
            {

                //serialize into JSON
                //Send the request to the API

                string url = "MenuData/UpdateMenu/" + id;


                string jsonpayload = jss.Serialize(Menu);
                Debug.WriteLine(jsonpayload);

                HttpContent content = new StringContent(jsonpayload);
                content.Headers.ContentType.MediaType = "application/json";

                //POST: api/MenuData/UpdateMenu/{id}
                //Header : Content-Type: application/json
                HttpResponseMessage response = client.PostAsync(url, content).Result;

                return RedirectToAction("Details/" + id);
            }
            catch
            {
                return View();
            }
        }


        // GET: Menu/Delete/5

        public ActionResult DeleteConfirm(int id)
        {
            string url = "MenuData/FindMenu/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            MenuDto selectedMenu = response.Content.ReadAsAsync<MenuDto>().Result;
            return View(selectedMenu);
        }

        // POST: Menu/Delete/5
        [HttpPost]

        public ActionResult Delete(int id)
        {
            string url = "MenuData/DeleteMenu/" + id;
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