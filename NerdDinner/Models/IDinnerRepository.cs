using System.Linq;

namespace NerdDinner.Models
{
    public interface IDinnerRepository
    {
        IQueryable<Dinner> FindAllDinners();
        IQueryable<Dinner> FindUpcomingDinners();
        IQueryable<Dinner> FindDinnersByText(string q);
        Dinner GetDinner(int id);
        void Add(Dinner dinner);
        void Delete(Dinner dinner);
        void Save();
    }
}