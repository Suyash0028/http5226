using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace n01629153_Event_Management.Models.ViewModel
{
    public class UpdateEvent
    {
        //This viewmodel is a class which stores information that we need to present to /Event/Update/{}

        //the existing event information

        public EventDto SelectedEvent { get; set; }

        // all sponsor to choose from when updating this event

        public IEnumerable<SponsorDto> SponsorOptions { get; set; }
        //public IEnumerable<UserDto> UserOptions { get; set; }
    }
}