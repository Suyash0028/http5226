using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventCateringManagementSystem.Models.ViewModel
{
    public class DetailEvent
    {
        public bool IsAdmin { get; set; }
        public EventDto SelectedEvent { get; set; }
        public List<MenuDto> SelectedMenus { get; set; }

        public List<MenuDto> AvailableMenus { get; set; }
    }
}