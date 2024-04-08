using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventCateringManagementSystem.Models.ViewModel
{
    public class DetailMenu
    {
        public bool IsAdmin { get; set; }
        public MenuDto SelectedMenu { get; set; }
        public List<MenuxFoodDto> SelectedFood { get; set; }
        public List<Food> AvailableFood { get; set; }
    }
}