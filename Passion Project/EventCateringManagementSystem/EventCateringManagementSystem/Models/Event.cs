using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EventCateringManagementSystem.Models
{
    public class Event
    {
        [Key]
        public int EventId { get; set; }
        public string EventName { get; set; }
        public string EventDescription { get; set; }
        public DateTime EventDate { get; set; }
        public string EventLocation { get; set; }

        // One-to-Many relationship with Menu
        public virtual ICollection<Menu> Menus { get; set; }
    }

    public class EventDto
    {
        public int EventId { get; set; }
        public string EventName { get; set; }
        public string EventDescription { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EventDate { get; set; }
        public string EventLocation { get; set; }
    }
}