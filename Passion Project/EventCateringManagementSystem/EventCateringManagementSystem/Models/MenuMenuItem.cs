using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventCateringManagementSystem.Models
{
    public class MenuMenuItem
    {
        //This is the bridging table for Menu and MenuItem
        public int MenuID { get; set; }
        public int MenuItemID { get; set; } 


        public virtual Menu Menu { get; set; }
        public virtual MenuItem MenuItem { get; set; }
    }
}