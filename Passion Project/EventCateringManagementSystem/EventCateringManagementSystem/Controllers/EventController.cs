using EventCateringManagementSystem.Models;
using EventCateringManagementSystem.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace EventCateringManagementSystem.Controllers
{
    public class EventController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static EventController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44377/api/");
        }

        // GET: Event
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


        // GET: Event/List
        public ActionResult List()
        {
            //objective: communicate with our event data api to retrieve a list of events
            //curl https://localhost:44377/api/EventData/ListEvents

            string url = "EventData/ListEvents";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<EventDto> events = response.Content.ReadAsAsync<IEnumerable<EventDto>>().Result;

            return View(events);
        }

        // GET: Event/Details/5
        public ActionResult Details(int id)
        {
            //objective: communicate with our Event data api to retrieve one Event
            //curl https://localhost:44377/api/Eventdata/FindEvent/{id}

            DetailEvent ViewModel = new DetailEvent();

            if (User.Identity.IsAuthenticated && User.IsInRole("Admin")) ViewModel.IsAdmin = true;
            else ViewModel.IsAdmin = false;


            string url = "EventData/FindEvent/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            EventDto selectedEvent = response.Content.ReadAsAsync<EventDto>().Result;


            ViewModel.SelectedEvent = selectedEvent;
            //show associated keepers with this animal
            url = "MenuData/ListMenusForEvents/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<MenuDto> SelectedMenus = response.Content.ReadAsAsync<IEnumerable<MenuDto>>().Result;

            ViewModel.SelectedMenus = SelectedMenus;

            url = "MenuData/ListAvailableMenusForEvents/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<MenuDto> AvailableMenus = response.Content.ReadAsAsync<IEnumerable<MenuDto>>().Result;

            ViewModel.AvailableMenus = AvailableMenus;


            return View(ViewModel);
        }

        public ActionResult Error()
        {
            return View();
        }

        // GET: Event/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Event/Create
        [HttpPost]
        
        public ActionResult Create(Event Event)
        {
            Debug.WriteLine("the json payload is :");
            //Debug.WriteLine(Event.EventName);
            //objective: add a new Event into our system using the API
            //curl -H "Content-Type:application/json" -d @Event.json https://localhost:44377/api/Eventdata/addEvent 
            string url = "EventData/AddEvent";


            string jsonpayload = jss.Serialize(Event);

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

        // GET: Event/Edit/5
        
        public ActionResult Edit(int id)
        {
            //grab the Event information

            //objective: communicate with our Event data api to retrieve one Event
            //curl https://localhost:44377/api/Eventdata/FindEvent/{id}

            UpdateEvent ViewModel = new UpdateEvent();
            string url = "EventData/FindEvent/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            EventDto selectedEvent = response.Content.ReadAsAsync<EventDto>().Result;
            ViewModel.SelectedEvent = selectedEvent;
            ViewBag.EventDate = Convert.ToDateTime(selectedEvent.EventDate).ToString("yyyy-MM-dd");


            return View(ViewModel);
        }

        // POST: Event/Update/5
        [HttpPost]
        
        public ActionResult Update(int id, Event Event)
        {
            try
            {

                //serialize into JSON
                //Send the request to the API

                string url = "EventData/UpdateEvent/" + id;


                string jsonpayload = jss.Serialize(Event);
                Debug.WriteLine(jsonpayload);

                HttpContent content = new StringContent(jsonpayload);
                content.Headers.ContentType.MediaType = "application/json";

                //POST: api/EventData/UpdateEvent/{id}
                //Header : Content-Type: application/json
                HttpResponseMessage response = client.PostAsync(url, content).Result;

                return RedirectToAction("Details/" + id);
            }
            catch
            {
                return View();
            }
        }

        //POST: Event/Associate/{Eventid}
        [HttpPost]
        
        public ActionResult Associate(int id, int MenuID)
        {
            GetApplicationCookie();//get token credentials
            Debug.WriteLine("Attempting to associate Event :" + id + " with Menu " + MenuID);

            //call our api to associate Event with Menu
            string url = "Eventdata/AssociateEventwithMenu/" + id + "/" + MenuID;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("Details/" + id);
        }


        //Get: Event/UnAssociate/{id}?MenuID={MenuID}
        [HttpGet]
        
        public ActionResult UnAssociate(int id, int MenuID)
        {
            GetApplicationCookie();//get token credentials
            Debug.WriteLine("Attempting to unassociate Event :" + id + " with Menu: " + MenuID);

            //call our api to unassociate Event with Menu
            string url = "Eventdata/unassociateEventwithMenu/" + id + "/" + MenuID;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("Details/" + id);
        }


        // GET: Event/Delete/5
        
        public ActionResult DeleteConfirm(int id)
        {
            string url = "EventData/FindEvent/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            EventDto selectedevent = response.Content.ReadAsAsync<EventDto>().Result;
            return View(selectedevent);
        }

        // POST: Event/Delete/5
        [HttpPost]
        
        public ActionResult Delete(int id)
        {
            string url = "EventData/DeleteEvent/" + id;
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