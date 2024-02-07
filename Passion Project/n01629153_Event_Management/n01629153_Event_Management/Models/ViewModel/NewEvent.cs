using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace n01629153_Event_Management.Models.ViewModel
{
    public class NewEvent
    {
        public IEnumerable<SponsorDto> SponsorOptions { get; set; }
        public IEnumerable<UserDto> UserOptions { get; set; }
    }
}