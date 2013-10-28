using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NerdDinner.Models
{
    [MetadataType(typeof(DinnerValidation))]
    public class Dinner
    {
        public int DinnerID { get; set; }

        public string Title { get; set; }

        public DateTime EventDate { get; set; }
        public string Address { get; set; }

        public string HostedBy { get; set; }

        public string Description { get; set; }

        public string ContactPhone { get; set; }
        public string Country { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }

        public virtual ICollection<RSVP> RSVPs { get; set; }

        public Dinner()
        {
            this.RSVPs = new Collection<RSVP>();
        }

        public bool IsHostedBy(string userName)
        {
            return userName.ToLower() == HostedBy.ToLower();
        }

    }
}