using EventCateringManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventCateringManagementSystem.Models.ViewModel
{
    public class UpdateEvent
    {
        //This viewmodel is a class which stores information that we need to present to /Event/Update/{}

        //the existing event information

        public EventDto SelectedEvent { get; set; }

    }
}