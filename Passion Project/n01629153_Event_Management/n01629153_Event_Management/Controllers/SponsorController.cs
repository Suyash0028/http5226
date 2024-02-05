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
    public class SponsorController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static SponsorController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44349/api/SponsorData/");
        }

        // GET: Sponsor
        public ActionResult Index()
        {
            return View();
        }

        // GET: Sponsor/List
        public ActionResult List()
        {
            //objective: communicate with our Sponsor data api to retrieve a list of sponsors
            //curl https://localhost:44349/api/SponsorData/ListSponsors

            string url = "ListSponsors";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<SponsorDto> sponsors = response.Content.ReadAsAsync<IEnumerable<SponsorDto>>().Result;

            return View(sponsors);
        }

        // GET: Sponsor/Details/5
        public ActionResult Details(int id)
        {
            //objective: communicate with our Sponsor data api to retrieve one Sponsor
            //curl https://localhost:44349/api/SponsorData/FindSponsor/{id}

            string url = "FindSponsor/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            SponsorDto selectedEvent = response.Content.ReadAsAsync<SponsorDto>().Result;

            return View(selectedEvent);
        }

        public ActionResult Error()
        {
            return View();
        }

        // GET: Sponsor/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Sponsor/Create
        [HttpPost]
        public ActionResult Create(Sponsor Sponsor)
        {
            Debug.WriteLine("the json payload is :");
            //Debug.WriteLine(Sponsor.EventName);
            //objective: add a new Sponsor into our system using the API
            //curl -H "Content-Type:application/json" -d @Sponsor.json https://localhost:44349/api/SponsorData/AddSponsor 
            string url = "AddSponsor";


            string jsonpayload = jss.Serialize(Sponsor);

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

        // GET: Sponsor/Edit/5
        public ActionResult Edit(int id)
        {
            //grab the Sponsor information

            //objective: communicate with our Sponsor data api to retrieve one Sponsor
            //curl https://localhost:44349/api/SponsorData/FindSponsor/{id}

            string url = "FindSponsor/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            SponsorDto selectedEvent = response.Content.ReadAsAsync<SponsorDto>().Result;
            //Debug.WriteLine("Sponsor received : ");
            //Debug.WriteLine(selectedEvent.EventName);

            return View(selectedEvent);
        }

        // POST: Sponsor/Update/5
        [HttpPost]
        public ActionResult Update(int id, Sponsor Sponsor)
        {
            try
            {

                //serialize into JSON
                //Send the request to the API

                string url = "UpdateSponsor/" + id;


                string jsonpayload = jss.Serialize(Sponsor);
                Debug.WriteLine(jsonpayload);

                HttpContent content = new StringContent(jsonpayload);
                content.Headers.ContentType.MediaType = "application/json";

                //POST: api/SponsorData/UpdateSponsor/{id}
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
            string url = "FindSponsor/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            SponsorDto selectedanimal = response.Content.ReadAsAsync<SponsorDto>().Result;
            return View(selectedanimal);
        }

        // POST: Animal/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "DeleteSponsor/" + id;
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