using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventCateringManagementSystem.Models.ViewModel
{
    public class DetailFood
    {
        public bool IsAdmin { get; set; }
        public FoodDto SelectedFood { get; set; }
    }
}