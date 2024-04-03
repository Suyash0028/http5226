using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventCateringManagementSystem.Models.ViewModel
{
    public class UpdateFood
    {
        //This viewmodel is a class which stores information that we need to present to /Food/Update/{id}

        //the existing Food information

        public FoodDto SelectedFood { get; set; }
    }
}