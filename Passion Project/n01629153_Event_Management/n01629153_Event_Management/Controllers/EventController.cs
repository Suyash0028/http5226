using n01629153_Event_Management.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace n01629153_Event_Management.Controllers
{
    public class EventController : Controller
    {

        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static EventController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44349/api/");
        }

        // GET: Event
        public ActionResult Index()
        {
            return View();
        }

        // GET: Event/List
        public ActionResult List()
        {
            //objective: communicate with our event data api to retrieve a list of events
            //curl https://localhost:44349/api/EventData/ListEvents

            string url = "EventData/ListEvents";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<EventDto> events = response.Content.ReadAsAsync<IEnumerable<EventDto>>().Result;
          
            return View(events);
        }

        // GET: Event/Details/5
        public ActionResult Details(int id)
        {
            //objective: communicate with our Event data api to retrieve one Event
            //curl https://localhost:44349/api/Eventdata/FindEvent/{id}

            string url = "EventData/FindEvent/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            EventDto selectedEvent = response.Content.ReadAsAsync<EventDto>().Result;

            return View(selectedEvent);
        }

        public ActionResult Error()
        {
            return View();
        }

        // GET: Event/New
        public ActionResult New()
        {
            //information about all species in the system.
            //GET api/speciesdata/listspecies

            string url = "SponsorData/ListSponsors";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<SponsorDto> sponsors = response.Content.ReadAsAsync<IEnumerable<SponsorDto>>().Result;

            return View(sponsors);
        }

        // POST: Event/Create
        [HttpPost]
        public ActionResult Create(Event Event)
        {
            Debug.WriteLine("the json payload is :");
            //Debug.WriteLine(Event.EventName);
            //objective: add a new Event into our system using the API
            //curl -H "Content-Type:application/json" -d @Event.json https://localhost:44349/api/Eventdata/addEvent 
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
            //return RedirectToAction("List");
        }

        // GET: Event/Edit/5
        public ActionResult Edit(int id)
        {
            //grab the Event information

            //objective: communicate with our Event data api to retrieve one Event
            //curl https://localhost:44349/api/Eventdata/FindEvent/{id}

            string url = "EventData/FindEvent/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            EventDto selectedEvent = response.Content.ReadAsAsync<EventDto>().Result;
            //Debug.WriteLine("Event received : ");
            //Debug.WriteLine(selectedEvent.EventName);

            return View(selectedEvent);
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

        // GET: Animal/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "EventData/FindEvent/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            EventDto selectedanimal = response.Content.ReadAsAsync<EventDto>().Result;
            return View(selectedanimal);
        }

        // POST: Animal/Delete/5
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