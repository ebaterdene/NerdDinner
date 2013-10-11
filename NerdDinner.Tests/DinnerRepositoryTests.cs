using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using FizzWare.NBuilder;
using NUnit.Framework;
using NerdDinner.Models;

namespace NerdDinner.Tests
{
    [TestFixture]
    public class DinnerRepositoryTests
    {
         [SetUp]
         public void BeforeEachTestRuns()
         {
             using (var db = new NerdDinners())
             {
                 if (db.Database.Exists())
                     db.Database.Delete();
             }

             this.Repository = new DinnerRepository();

             var dinners = Builder<Dinner>
                 .CreateListOfSize(10)
                 .All()
                 .Do(row =>
                     {
                         var rsvPs = Builder<RSVP>.CreateListOfSize(5).Build().ToList();
                         row.RSVPs = rsvPs;
                     })
                 .Build()
                 ;

             using (var db = new NerdDinners())
             {
                 foreach (var dinner in dinners)
                 {
                     db.Dinners.Add(dinner);

                     foreach (var rsvp in dinner.RSVPs)
                     {
                         db.RSVPs.Add(rsvp);
                     }

                 }
                 db.SaveChanges();
             }

             this.AllDinners = dinners.ToList();

         }

        protected List<Dinner> AllDinners { get; set; }

        protected DinnerRepository Repository { get; set; }
    
        [Test]
        public void FindAllDinners()
        {
            // Arrange

            // Act
            var results = this.Repository.FindAllDinners().ToList();

            // Assert
            Assert.That(results, Has.Count.EqualTo(this.AllDinners.Count));
        }

        [Test]
        public void FindUpcomingDinners()
        {
            // Arrange

            // Act
            var results = this.Repository.FindUpcomingDinners().ToList();

            // Assert
            Assert.That(results, Has.Count.EqualTo(this.AllDinners.Count -1));

            var dinnersThatShouldNotBeReturned = this.AllDinners
                .Where(row => row.EventDate < DateTime.Now)
                .ToList()
                ;

            Assert.That(dinnersThatShouldNotBeReturned, Has.Count.GreaterThan(0), "If there are no dinners before now then this isn't a valid test.");

            foreach (var dinner in dinnersThatShouldNotBeReturned)
            {
                var actual = results.SingleOrDefault(row => row.DinnerID == dinner.DinnerID);
                Assert.That(actual, Is.Null);
            }

        }

        [Test]
        public void GetDinner()
        {
            // Arrange
            var id = this.AllDinners.First().DinnerID;

            // Act
            var dinner = this.Repository.GetDinner(id);

            // Assert
            Assert.That(dinner, Is.Not.Null);

            Assert.That(dinner.DinnerID, Is.EqualTo(id));
        }

        [Test]
        public void GetDinner_WhenNoDinnerExistsWithSpecifiedId()
        {
            // Arrange
            var id = this.AllDinners.Select(row => row.DinnerID).Max() + 1;

            // Act
            var dinner = this.Repository.GetDinner(id);

            // Assert
            Assert.That(dinner, Is.Null);
        }

        [Test]
        public void AddDinner()
        {
            // Arrange
            var dinner = Builder<Dinner>.CreateNew().Build();

            // Act
            this.Repository.Add(dinner);
            this.Repository.Save();

            // Assert
            var results = this.Repository.FindAllDinners().ToList();

            Assert.That(results, Has.Count.EqualTo(this.AllDinners.Count + 1));
        }

        [Test]
        public void DeleteDinner()
        {
            // Arrange
            var dinner = this.AllDinners.First();

            // Act
            this.Repository.Delete(dinner);
            this.Repository.Save();

            // Assert
            var results = this.Repository.FindAllDinners().ToList();

            Assert.That(results, Has.Count.EqualTo(this.AllDinners.Count - 1));
        }
    }
}