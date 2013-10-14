using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NerdDinner.Models
{
    public class Dinner
    {
        public int DinnerID { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public DateTime EventDate { get; set; }
        public string Address { get; set; }
        [Required]
        public string HostedBy { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string ContactPhone { get; set; }
        public string Country { get; set; }
        public string Latitude { get; set; }
        public string Longtitude { get; set; }

        public virtual ICollection<RSVP> RSVPs { get; set; }

        public Dinner()
        {
            this.RSVPs = new Collection<RSVP>();
        }
    }
}