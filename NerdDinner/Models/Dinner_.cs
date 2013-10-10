//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NerdDinner.Models
{
    using System;
    using System.Collections.Generic;
    //[MetadataType(typeof(Dinner_Validation))]
    public partial class Dinner
    {
        public Dinner()
        {
            this.RSVPs = new HashSet<RSVP>();
        }
    
        public int DinnerID { get; set; }
        public string Title { get; set; }
        public System.DateTime EventDate { get; set; }
        public string Description { get; set; }
        public string HostedBy { get; set; }
        public string ContactPhone { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public double Latitude { get; set; }
        public double Longtitude { get; set; }
    
        public virtual ICollection<RSVP> RSVPs { get; set; }
    }

    public class Dinner_Validation
    {
        /*
        [Required(ErrorMessage = "Title is required")]
        [StringLength(50, ErrorMessage = "Title may not be longer than 50 characters")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(265, ErrorMessage = �Description must be 256 characters or less�)]
        public string Description { get; set; }
        [Required(ErrorMessage = �Address is required�)]
        public string Address { get; set; }
        [Required(ErrorMessage = �Country is required�)]
        public string Country { get; set; }
        [Required(ErrorMessage = �Phone# is required�)]
        public string ContactPhone { get; set; }
        */
    }
}