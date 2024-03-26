using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EventCateringManagementSystem.Models
{
    public class Food
    {
        [Key]
        public int FoodID { get; set; }
        public string FoodName { get; set; }
        public string FoodDescription { get; set; }
        public decimal FoodPrice { get; set; }

        //Many-to-Many relation with Menu
        public virtual ICollection<MenuxFood> Foods { get; set; }
    }

    public class FoodDto
    {
        public int FoodID { get; set; }
        public string FoodName { get; set; }
        public string FoodDescription { get; set; }
        public decimal FoodPrice { get; set; }
    }
}