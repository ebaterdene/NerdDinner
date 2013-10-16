using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FizzWare.NBuilder;
using NUnit.Framework;
using NerdDinner.Models;

namespace NerdDinner.Tests.Models
{
    [TestFixture]
    public class NerdDinnersTests
    {
        [Test]
        public void InsertDinner()
        {
            // Arrange
            int? dinnerId = null;
            using (var db = new NerdDinners())
            {
                var dinner = Builder<Dinner>
                    .CreateNew()
                    .Build()
                    ;

                db.Dinners.Add(dinner);

                // Act
                db.SaveChanges();

                dinnerId = dinner.DinnerID;
            }

            // Assert
            using (var db = new NerdDinners())
            {
                var dinner = db.Dinners.SingleOrDefault(arg => arg.DinnerID == dinnerId);
                Assert.That(dinner, Is.Not.Null);
            }

        }

        [Test]
        public void InsertRSVP()
        {
            // Arrange
            int? dinnerId = null;
            int? rsvpId = null;
            using (var db = new NerdDinners())
            {
                var dinner = Builder<Dinner>
                    .CreateNew()
                    .Build()
                    ;

                var rsvp = Builder<RSVP>
                    .CreateNew()
                    .Build()
                    ;

                rsvp.Dinner = dinner;

                db.Dinners.Add(dinner);
                db.RSVPs.Add(rsvp);

                // Act
                db.SaveChanges();

                dinnerId = dinner.DinnerID;
                rsvpId = rsvp.RsvpID;
            }


            // Assert
            using (var db = new NerdDinners())
            {
                var dinner = db.Dinners.SingleOrDefault(arg => arg.DinnerID == dinnerId);
                Assert.That(dinner, Is.Not.Null);

                var rsvp = dinner.RSVPs.SingleOrDefault(arg => arg.RsvpID == rsvpId);
                Assert.That(rsvp, Is.Not.Null);
                Assert.That(rsvp.Dinner, Is.SameAs(dinner));
            }



        }

    }
}
