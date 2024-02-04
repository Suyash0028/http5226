using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace n01629153_Event_Management.Models
{
    public class Sponsor
    {

        [Key]
        public int SponsorId { get; set; }
        public string SponsorName { get; set; }
        public string ContactPerson { get; set; }
        public string ContactEmail { get; set; }

        // A sponcor can have multiple events
        public ICollection<Event> Events { get; set; }

    }
    public class SponsorDto
    {
        public int SponsorId { get; set; }
        public string SponsorName { get; set; }
        public string ContactPerson { get; set; }
        public string ContactEmail { get; set; }
    }
}