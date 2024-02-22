using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace n01629153_Event_Management.Models.ViewModel
{
    public class DetailEvent
    {
        public SponsorDto SelectedSponsor { get; set; }

        // All events for a particular sponsor
        public IEnumerable<EventDto> SponsorEvents { get; set; }

        public UserDto SelectedUser { get; set; }

        // All events for a particular sponsor
        public IEnumerable<EventDto> UserEvents { get; set; }
    }
}