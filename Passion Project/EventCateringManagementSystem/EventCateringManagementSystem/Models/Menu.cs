using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EventCateringManagementSystem.Models
{
    public class Menu
    {
        public int MenuID { get; set; }
        public string MenuTitle { get; set; }
        public string MenuDescription { get; set; }
        [ForeignKey("EventID")]
        public int EventID { get; set; }
        public virtual Event Event { get; set; }

        //Many-to-Many relationship with MenuItem
        public virtual ICollection<MenuMenuItem> MenuMenuItems { get; set;}
        
    }
}