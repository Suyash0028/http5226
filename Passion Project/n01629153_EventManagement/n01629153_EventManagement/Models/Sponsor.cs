using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace n01629153_EventManagement.Models
{
    public class Sponsor
    {
        [Key]
        public int SponsorId { get; set; }
        public string SponsorName { get; set; }
        public string ContactPerson { get; set; }
        public string ContactEmail { get; set; }
        [ForeignKey("Events")]
        public int EventId { get; set; }
        public virtual Event Events { get; set; }
    }
}