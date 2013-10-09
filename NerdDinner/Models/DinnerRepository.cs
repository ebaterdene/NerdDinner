using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NerdDinner.Models
{
    public class DinnerRepository
    {
        //private NerdDinnerEntities entities = new NerdDinnerEntities();
        private NerDinnerEntities entities = new NerDinnerEntities();
        // Query Methods.

        public IQueryable<Dinner> FindAllDinners()
        {
            return entities.Dinners;
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
            return entities.Dinners.FirstOrDefault(d => d.DinnerID == id);
        }

        public void Add(Dinner dinner)
        {
            entities.Dinners.Add(dinner);
        }

        public void Delete(Dinner dinner)
        {
            foreach (var rsvp in dinner.RSVPs)
            {
                entities.RSVPs.Remove(rsvp);
            }
            entities.Dinners.Remove(dinner);
        }

        //Persistence
        public void Save()
        {
            entities.SaveChanges();
        }

    }
}