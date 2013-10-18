using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FizzWare.NBuilder;
using NUnit.Framework;
using NerdDinner.Controllers;
using NerdDinner.Models;

namespace NerdDinner.Tests.Models
{
    [TestFixture]
    public class DinnerFormViewModelTests
    {
        [Test]
        public void ctor()
        {
            // Arrange
            var dinner = Builder<Dinner>
                .CreateNew()
                .Build()
                ;

            // Act
            var model = new DinnerFormViewModel(dinner);

            // Assert
            Assert.That(model.DinnerID, Is.EqualTo(dinner.DinnerID), "DinnerID");
            
            Assert.That(model.Address, Is.EqualTo(dinner.Address), "Address");
            Assert.That(model.HostedBy, Is.EqualTo(dinner.HostedBy), "HostedBy");
            Assert.That(model.ContactPhone, Is.EqualTo(dinner.ContactPhone), "ContactPhone");
            Assert.That(model.Country, Is.EqualTo(dinner.Country), "Country");
            Assert.That(model.Description, Is.EqualTo(dinner.Description), "Description");
            Assert.That(model.EventDate, Is.EqualTo(dinner.EventDate), "EventDate");
            Assert.That(model.Latitude, Is.EqualTo(dinner.Latitude), "Latitude");
            Assert.That(model.Longtitude, Is.EqualTo(dinner.Longtitude), "Longtitude");
            Assert.That(model.Title, Is.EqualTo(dinner.Title), "Title");
        }
    }
}
