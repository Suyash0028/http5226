using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace n01629153_EventManagement.Models
{
    public class Event
    {
        [Key]
        public int EventId { get; set; }
        public string EventName { get; set; }
        public string EventDescription { get; set; } = string.Empty;
        public DateTime EventDate { get; set; }
        public string EventLocation { get; set; }
        [ForeignKey("EventUsers")]
        public int UserId { get; set; }
        public virtual User EventUsers { get; set; }
    }
}