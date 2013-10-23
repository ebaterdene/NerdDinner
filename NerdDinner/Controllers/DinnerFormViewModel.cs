using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NerdDinner.Models;

namespace NerdDinner.Controllers
{
    public class DinnerFormViewModel
    {
        private static string[] _countries = new string[]
            {
                "USA",
                "Afghanistan",
                "Akrotiri",
                "Albania",
                "Zimbabwe"
            };

        public SelectList Countries { get; private set; }

        public int DinnerID { get; set; }

        [Required]
        public string Title { get; set; }

        public DateTime EventDate { get; set; }

        public string Address { get; set; }

        public string HostedBy { get; set; }

        public string Description { get; set; }

        public string ContactPhone { get; set; }

        public string Country { get; set; }

        public string Latitude { get; set; }

        public string Longtitude { get; set; }

        public DinnerFormViewModel(Dinner dinner)
        {
            Countries = new SelectList(_countries, dinner.Country);
            this.DinnerID = dinner.DinnerID;
            this.Address = dinner.Address;
            this.HostedBy = dinner.HostedBy;
            this.Title = dinner.Title;
            this.Description = dinner.Description;
            this.EventDate = dinner.EventDate;
            this.ContactPhone = dinner.ContactPhone;
            this.Country = dinner.Country;
            this.Longtitude = dinner.Longtitude;
            this.Latitude = dinner.Latitude;
        }

        public DinnerFormViewModel()
        {
            Countries = new SelectList(_countries);   
        }
    }
}