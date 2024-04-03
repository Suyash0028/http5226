using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventCateringManagementSystem.Models.ViewModel
{
    public class UpdateMenu
    {
        //This viewmodel is a class which stores information that we need to present to /Menu/Update/{id}

        //the existing menu information

        public MenuDto SelectedMenu { get; set; }
    }
}