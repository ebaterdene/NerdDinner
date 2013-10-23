using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NerdDinner.Models
{
    public class DinnerRepository : IDinnerRepository
    {
        //private NerdDinnerEntities entities = new NerdDinnerEntities();
        private NerdDinners entities = new NerdDinners();
        // Query Methods.

        public IQueryable<Dinner> FindAllDinners()
        {
            return entities.Dinners;
        }


        public IQueryable<Dinner> FindDinnersByText(string q)
        {
            return entities.Dinners.Where(d => d.Title.Contains(q)
                            || d.Description.Contains(q)
                            || d.HostedBy.Contains(q));
        }

        public IQueryable<Dinner> FindUpcomingDinners()
        {
            return from dinner in entities.Dinners
                    where dinner.EventDate > DateTime.Now
                    orderby dinner.EventDate
                    select dinner;
        }


        public Dinner GetDinner(int id)
        {
            //returns null if dinner not found.
            return entities.Dinners.FirstOrDefault(d => d.DinnerID == id);;
        }

        public void Add(Dinner dinner)
        {
            entities.Dinners.Add(dinner);
        }



        public void Delete(Dinner dinner)
        {
            var dinnerToRemove = this.entities.Dinners.Single(row => row.DinnerID == dinner.DinnerID);
            var rsvpsToRemove = dinnerToRemove.RSVPs.ToList();

            foreach (var rsvp in rsvpsToRemove)
            {
                entities.RSVPs.Remove(rsvp);

            }
            entities.Dinners.Remove(dinnerToRemove);
        }

        //Persistence
        public void Save()
        {
            entities.SaveChanges();
        }

    }
}