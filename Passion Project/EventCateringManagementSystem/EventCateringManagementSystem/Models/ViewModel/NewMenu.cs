using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventCateringManagementSystem.Models.ViewModel
{
    public class NewMenu
    {
        public IEnumerable<EventDto> EventOptions { get; set; }
    }
}