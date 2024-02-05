using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace n01629153_Event_Management.Models
{
    public class Event
    {
        [Key]
        public int EventId { get; set; }
        public string EventName { get; set; }
        public string EventDescription { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime EventDate { get; set; }
        public string EventLocation { get; set; }

        [ForeignKey("EventUsers")]
        public int UserId { get; set; }
        public virtual User EventUsers { get; set; }

        [ForeignKey("Sponsors")]
        public int SponsorId { get; set; }
        public virtual Sponsor Sponsors { get; set; }
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
        public string UserName { get; set; }
        public int SponsorId { get; set; }
        public string SponsorName { get; set; }
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}