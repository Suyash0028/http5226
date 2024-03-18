using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventCateringManagementSystem.Models
{
    public class MenuItem
    {
        public int MenuItemID { get; set; } 
        public string MenuItemName { get; set; }
        public string MenuItemDescription { get; set; }
        public decimal MenuItemPrice { get; set; }

        //Many-to-Many relation with Menu
        public virtual ICollection<MenuMenuItem> MenuMenuItems { get; set; }
    }
}