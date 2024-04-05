using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EventCateringManagementSystem.Models
{
    public class Menu
    {
        [Key]
        public int MenuID { get; set; }
        public string MenuTitle { get; set; }
        public string MenuDescription { get; set; }
        [ForeignKey("Event")]
        public int EventId { get; set; }
        public virtual Event Event { get; set; }

        //Many-to-Many relationship with MenuItem
        public virtual ICollection<MenuxFood> Menus { get; set; }
    }

    public class MenuDto
    {
        public int MenuID { get; set; }
        public string MenuTitle { get; set; }
        public string MenuDescription { get; set; }
        public int EventId { get; set; }
        public string EventName { get; set; }
        public string EventDescription { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EventDate { get; set; }
        public string EventLocation { get; set; }
        public List<MenuxFood> Menus { get; set; }
    }
}