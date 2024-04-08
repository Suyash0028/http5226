using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EventCateringManagementSystem.Models
{
    public class MenuxFood
    {
        //This is the bridging table for Menu and Food
        [Key]
        public int MenuxFoodID { get; set; }
        [ForeignKey("Menu")]
        public int MenuID { get; set; }
        public virtual Menu Menu { get; set; }

        [ForeignKey("Food")]
        public int FoodID { get; set; }
        public virtual Food Food { get; set; }

        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
    public class MenuxFoodDto
    {
        public int MenuxFoodID { get; set; }
        public int MenuID { get; set; }
        public string MenuTitle { get; set; }
        public string MenuDescription { get; set; }
        public int FoodID { get; set; }
        public string FoodName { get; set; }
        public string FoodDescription { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}