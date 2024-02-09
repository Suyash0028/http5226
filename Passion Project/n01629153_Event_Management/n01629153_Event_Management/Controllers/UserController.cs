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
    public class UserController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static UserController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44349/api/");
        }

        // GET: User/List
        public ActionResult List()
        {
            //objective: communicate with our User data api to retrieve a list of Users
            //curl https://localhost:44349/api/UserData/ListUsers

            string url = "UserData/ListUsers";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<UserDto> Users = response.Content.ReadAsAsync<IEnumerable<UserDto>>().Result;
            //Debug.WriteLine("Number of Users received : ");
            //Debug.WriteLine(Users.Count());

            return View(Users);
        }

        // GET: User/Details/5
        public ActionResult Details(int id)
        {
            //objective: communicate with our User data api to retrieve one User
            //curl https://localhost:44349/api/UserData/FindUser/{id}

            string url = "Userdata/FindUser/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is ");
            Debug.WriteLine(response.StatusCode);

            UserDto selectedUser = response.Content.ReadAsAsync<UserDto>().Result;
            Debug.WriteLine("User received : ");
            Debug.WriteLine(selectedUser.UserName);


            return View(selectedUser);
        }

        public ActionResult Error()
        {

            return View();
        }

        // GET: User/New
        public ActionResult New()
        {
            return View();
        }

        // POST: User/Create
        [HttpPost]
        public ActionResult Create(User User)
        {
            Debug.WriteLine("the json payload is :");
            //Debug.WriteLine(User.UserName);
            //objective: add a new User into our system using the API
            //curl -H "Content-Type:application/json" -d @User.json https://localhost:44349/api/UserData/AddUser 
            string url = "Userdata/AddUser";


            string jsonpayload = jss.Serialize(User);

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

        // GET: User/Edit/5
        public ActionResult Edit(int id)
        {
            //grab the User information

            //objective: communicate with our User data api to retrieve one User
            //curl https://localhost:44349/api/UserData/FindUser/{id}

            string url = "Userdata/FindUser/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            UserDto selectedUser = response.Content.ReadAsAsync<UserDto>().Result;
            //Debug.WriteLine("User received : ");
            //Debug.WriteLine(selectedUser.UserName);

            return View(selectedUser);
        }

        // POST: User/Update/5
        [HttpPost]
        public ActionResult Update(int id, User User)
        {
            try
            {
                //serialize into JSON
                //Send the request to the API

                string url = "Userdata/UpdateUser/" + id;


                string jsonpayload = jss.Serialize(User);
                Debug.WriteLine(jsonpayload);

                HttpContent content = new StringContent(jsonpayload);
                content.Headers.ContentType.MediaType = "application/json";

                //POST: api/UserData/UpdateUser/{id}
                //Header : Content-Type: application/json
                HttpResponseMessage response = client.PostAsync(url, content).Result;

                return RedirectToAction("Details/" + id);
            }
            catch
            {
                return View();
            }
        }

        // GET: User/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "Userdata/FindUser/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            UserDto selecteduser = response.Content.ReadAsAsync<UserDto>().Result;
            return View(selecteduser);
        }

        // POST: User/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "UserData/DeleteUser/" + id;
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

        // GET: User/ShowEvents
        public ActionResult ShowEvents(int id)
        {
            //objective: communicate with our Event data api to retrieve a list of events based on user id
            //curl https://localhost:44349/EventData/ListEventsForUser/

            string url = "EventData/ListEventsForUser/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<EventDto> users = response.Content.ReadAsAsync<IEnumerable<EventDto>>().Result;

            url = "UserData/FindUser/" + id;
            response = client.GetAsync(url).Result;
            UserDto selectedUser = response.Content.ReadAsAsync<UserDto>().Result;
            ViewBag.UserName = selectedUser.FirstName + " " + selectedUser.LastName;

            return View(users);
        }
    }
}