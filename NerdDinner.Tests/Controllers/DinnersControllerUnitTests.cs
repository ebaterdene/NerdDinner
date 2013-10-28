using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using FizzWare.NBuilder;
using NSubstitute;
using NSubstitute.Exceptions;
using NUnit.Framework;
using NerdDinner.Controllers;
using NerdDinner.Models;

namespace NerdDinner.Tests.Controllers
{
    [TestFixture]
    public class DinnersControllerUnitTests
    {
        [SetUp]
        public void BeforeEachTestRuns()
        {
            // Arrange
            //Moq IDinnerRepository Interface
            this.Repository = Substitute.For<IDinnerRepository>(); 
            // Conroller with Fake repository. Fake repository doesn't do anything
            this.Controller = new DinnersController(this.Repository); 

            var routeData = new RouteData();
            var httpContext = Substitute.For<HttpContextBase>();

            var controllerContext = Substitute.For<ControllerContext>(httpContext,
                                                                      routeData,
                                                                      this.Controller);
            this.Controller.ControllerContext = controllerContext;

            // Act

            // Assert

        }

        protected IDinnerRepository Repository { get; set; }

        protected DinnersController Controller { get; set; }

        [Test]
        [TestCase("Create", typeof(HttpGetAttribute))]
        [TestCase("Create", typeof(HttpPostAttribute))]
        [TestCase("Edit", null)]
        [TestCase("Edit", typeof(HttpGetAttribute))]
        [TestCase("Edit", typeof(HttpPostAttribute))]
        [TestCase("Delete", typeof(HttpGetAttribute))]
        [TestCase("DeletePost", typeof(HttpPostAttribute))]
        public void MethodRequiresAuthorizedUser(string methodName, Type httpVerb)
        {
            if (httpVerb == null) return; // ? HttpGet | HttpPost

            var methodInfos = this.Controller
                .GetType()
                .GetMethods()
                .Where(method => method.Name == methodName)
                .Where(method => httpVerb == null ? !method.GetCustomAttributes(inherit: true).Any() : method.GetCustomAttributes(httpVerb, true).Any())
                ;

            var methodInfo = methodInfos.Single();

            //var controller = new DinnersController(new DinnerRepository());
            //methodInfo.Invoke(controller, new object[] { 1 });

            var attributes = methodInfo.GetCustomAttributes(typeof(AuthorizeAttribute), inherit: true)
                .Cast<AuthorizeAttribute>()
                .ToList()
                ;

            Assert.That(attributes.Any());

        }


        [Test]
        [TestCase(-1, 1, 10)]
        [TestCase(0, 1, 10)]
        [TestCase(1, 11, 20)]
        [TestCase(2, 21, 30)]
        [TestCase(9, 91, 100)]
        [TestCase(10, 101, 105)]
        [TestCase(11, null, null)]
        public void Index_Get_Page(int page, int? minDinnerId, int? maxDinnerId)
        {
            // Arrange

            var dinners = Builder<Dinner>
                .CreateListOfSize(105)
                .Build()
                .AsQueryable()
                ;

            this.Repository.FindUpcomingDinners().Returns(dinners);

            // Act
            var result = (ViewResult)this.Controller.Index(page);
            var model = (IList<Dinner>)result.ViewData.Model;

            // Assert
            var dinnerIds = model.Select(row => row.DinnerID).ToList();
            var resultsAreExpected = minDinnerId.HasValue && maxDinnerId.HasValue;
            if (resultsAreExpected)
            {
                var count = maxDinnerId.Value + 1 - minDinnerId.Value;
                Assert.That(dinnerIds, Has.Count.EqualTo(count));

                var expectedRange = Enumerable.Range(minDinnerId.Value, count).ToArray();
                Assert.That(dinnerIds, Is.EquivalentTo(expectedRange));
            }
            else
            {
                Assert.That(model, Is.Empty);
            }

        }

        [Test]
        public void DetailsGet()
        {
            // Arrange
            var dinner = Builder<Dinner>
                .CreateNew()
                .Build()
                ;
            var dinnerId = dinner.DinnerID;
            this.Repository.GetDinner(dinnerId).Returns(dinner);
            
            // Act
            var result = this.Controller.Details(dinnerId) as ViewResult;
            var model = result.ViewData.Model as Dinner;

            // Assert
            Assert.IsNotNull(model);
            Assert.That(model, Is.SameAs(dinner));
            Assert.That(result, Is.Not.Null, "Controller did not return a valid ViewResult");
        }

        [Test]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(int.MaxValue)]
        public void DetailsGet_IdNotFound(int id)
        {
            // Arrange

            // Act
            var result = this.Controller.Details(id) as RedirectToRouteResult;

            // Assure
            Assert.That(result, Is.Not.Null, "Controller did not return a valid RedirectToRouteResult");
            Assert.That(result.RouteValues["action"], Is.EqualTo("NotFound"));
            Assert.That(result.RouteValues.ContainsKey("id"));
            Assert.That(result.RouteValues.ContainsValue(id));
        }

        // GET: Edit
        [Test]
        public void EditGet()
        {
            // Arrange
            var dinner = Builder<Dinner>.CreateNew().Build();

            var dinnerId = dinner.DinnerID;
            this.Repository.GetDinner(dinnerId).Returns(dinner);

            // Act
            var result = this.Controller.Edit(dinnerId) as ViewResult;
            
            // Assert
            var model = result.ViewData.Model as DinnerFormViewModel;
            Assert.IsNotNull(model, "model");
            Assert.That(model.DinnerID, Is.EqualTo(dinnerId), "Dinner ID");
            Assert.That(model.Title, Is.EqualTo(dinner.Title), "Title");
            Assert.That(model.EventDate, Is.EqualTo(dinner.EventDate), "Event Date");
        }

        [Test]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(int.MaxValue)]
        public void EditGet_NotFound(int id)
        {
            // Arrange
            
            // Act
            var result = this.Controller.Edit(id) as RedirectToRouteResult;

            // Assure
            Assert.That(result, Is.Not.Null);
            Assert.That(result.RouteName, Is.EqualTo(""));

            Assert.That(result.RouteValues["action"], Is.EqualTo("NotFound"));
            Assert.That(result.RouteValues["id"], Is.EqualTo(id));
        }

        [Test]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(21341234)]
        [TestCase(int.MaxValue)]
        public void EditPost(int dinnerId)
        {
            // Arrange
            var christmas = new DateTime(2013, 12, 25);
            var viewModel = Builder<DinnerFormViewModel>.CreateNew().Build();

            var dinner = Builder<Dinner>
                .CreateNew()
                .Build()
                ;

            this.Repository.GetDinner(viewModel.DinnerID).Returns(dinner);

            // Act
            var result = this.Controller.Edit(viewModel) as RedirectToRouteResult;
            
            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.RouteValues["action"], Is.EqualTo("Details"));
            Assert.That(result.RouteValues["id"], Is.EqualTo(viewModel.DinnerID));


            // There are Edited.
            Assert.That(dinner.Address, Is.EqualTo(viewModel.Address), "Address");
            Assert.That(dinner.ContactPhone, Is.EqualTo(viewModel.ContactPhone), "ContactPhone");
            Assert.That(dinner.Country, Is.EqualTo(viewModel.Country), "Country");
            Assert.That(dinner.Description, Is.EqualTo(viewModel.Description), "Description");
            Assert.That(dinner.EventDate, Is.EqualTo(viewModel.EventDate), "EventDate");
            Assert.That(dinner.HostedBy, Is.EqualTo(viewModel.HostedBy), "HostedBy");
            Assert.That(dinner.Latitude, Is.EqualTo(viewModel.Latitude), "Latitude");
            Assert.That(dinner.Longitude, Is.EqualTo(viewModel.Longtitude), "Longitude");
            Assert.That(dinner.Title, Is.EqualTo(viewModel.Title), "Title");

            this.Repository.Received().Save();
        }


        [Test]
        public void EditPost_WithModelStateErrors()
        {
            // Arrange
            var viewModel = Builder<DinnerFormViewModel>
                .CreateNew()
                .Build()
                ;

            // Act
            this.Controller.ModelState.AddModelError("Someproperty", "Some Message");
            var result = this.Controller.Edit(viewModel) as ViewResult;

            // Assure
            Assert.IsNotNull(result);
            Assert.That(result.Model, Is.EqualTo(viewModel));
        }

        [Test]
        public void CreateGet()
        {
            // Arrange

            // Act
            var result = this.Controller.Create() as ViewResult;
            var model = result.ViewData.Model as DinnerFormViewModel;

            // Assure
            Assert.IsNotNull(model);
            Assert.That(model.DinnerID, Is.EqualTo(0));
            Assert.That(model.Title, Is.Null);
            Assert.That(model.EventDate.ToShortDateString(),
                        Is.EqualTo(DateTime.Now.AddDays(7).ToShortDateString()));
        }


        [Test]
        public void CreatePost()
        {
            // Arrange
            var viewModel = Builder<DinnerFormViewModel>
                .CreateNew()
                .Build()
                ;
            Dinner dinnerThatWasAdded = null;
            const int dinnerId = 47;
            this.Repository.WhenForAnyArgs(row => row.Add(null))
                .Do(callInfo =>
                {
                    var dinner = callInfo.Arg<Dinner>();
                    dinner.DinnerID = dinnerId;
                    dinnerThatWasAdded = dinner;
                });

            // Act
            var result = this.Controller.Create(viewModel) as RedirectToRouteResult;

            // Assure
            Assert.IsNotNull(result);

            Assert.That(dinnerThatWasAdded, Is.Not.Null, "Call to Repository.Add() was not made.");
            Assert.That(dinnerThatWasAdded.Address, Is.EqualTo(viewModel.Address));
            Assert.That(dinnerThatWasAdded.ContactPhone, Is.EqualTo(viewModel.ContactPhone));
            Assert.That(dinnerThatWasAdded.Description, Is.EqualTo(viewModel.Description));
            Assert.That(dinnerThatWasAdded.HostedBy, Is.EqualTo(viewModel.HostedBy));
            Assert.That(dinnerThatWasAdded.EventDate, Is.EqualTo(viewModel.EventDate));
            Assert.That(dinnerThatWasAdded.Country, Is.EqualTo(viewModel.Country));
            Assert.That(dinnerThatWasAdded.Longitude, Is.EqualTo(viewModel.Longtitude));
            Assert.That(dinnerThatWasAdded.Latitude, Is.EqualTo(viewModel.Latitude));
            Assert.That(dinnerThatWasAdded.Title, Is.EqualTo(viewModel.Title));
            Repository.Received().Add(dinnerThatWasAdded);
            Repository.Received().Save();
            Assert.That(result.RouteValues["action"], Is.EqualTo("Details"));
            Assert.That(result.RouteValues["id"], Is.EqualTo(dinnerId));

        }

        [Test]
        public void CreatePost_WithModelStateErrors()
        {
            // Arrange
            var viewModel = Builder<DinnerFormViewModel>
                .CreateNew()
                .Build()
                ;

            // Act
            this.Controller.ModelState.AddModelError("SomeProperty", "SomeMessage");
            var result = this.Controller.Create(viewModel) as ViewResult;

            // Assure
            this.Repository.DidNotReceive().Add(Arg.Any<Dinner>());
            this.Repository.DidNotReceive().Save();

            Assert.IsNotNull(result);
            Assert.That(result.Model, Is.SameAs(viewModel));
        }



        [Test]
        public void DeleteGet()
        {
            // Arrange
            var dinner = Builder<Dinner>
                .CreateNew()
                .Build()
                ;
            var dinnerId = dinner.DinnerID;

            // Act
            this.Repository.GetDinner(dinnerId).Returns(dinner);
            var result = this.Controller.Delete(dinnerId) as ViewResult;
            var model = result.ViewData.Model as Dinner;

            // Assert
            Assert.NotNull(result);
            Assert.That(model, Is.SameAs(dinner));
        }

        [Test]
        public void DeleteGet_NotFound()
        {
            // Arrange 
            const int dinnerId = 123456;
            this.Repository.GetDinner(dinnerId).Returns((Dinner)null);

            // Act
            var result = this.Controller.Delete(dinnerId) as RedirectToRouteResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.RouteValues["action"], Is.EqualTo("NotFound"));
            Assert.That(result.RouteValues["id"], Is.EqualTo(dinnerId));
        }

        [Test]
        public void Delete_Post_Dinner_That_Exists()
        {
            // Arrange
            var dinner = Builder<Dinner>
                .CreateNew()
                .Build()
                ;
            var dinnerID = dinner.DinnerID;
            this.Repository.GetDinner(dinnerID).Returns(dinner);

            // Act
            var result = this.Controller.DeletePost(dinnerID) as ViewResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ViewName, Is.EqualTo("Deleted"));

            this.Repository
                .Received()
                .Delete(dinner)
                ;
            this.Repository
                .Received()
                .Save()
                ;

        }

        [Test]
        public void Delete_Post_Dinner_That_Does_Not_Exists()
        {
            const int dinnerIdFake = 546654654;
            this.Repository.GetDinner(dinnerIdFake).Returns((Dinner)null);

            // Act
            var result = this.Controller.DeletePost(dinnerIdFake) as RedirectToRouteResult;

            // Assert
            Assert.NotNull(result);
            Assert.That(result.RouteValues["action"], Is.EqualTo("NotFound"));
            Assert.That(result.RouteValues["id"], Is.EqualTo(dinnerIdFake));

            this.Repository.DidNotReceive().Delete(Arg.Any<Dinner>());
            this.Repository.DidNotReceive().Save();
        }

        [Test]
        public void NotFoundTest()
        {
            // Arrange
            var tempId = int.MaxValue;

            // Act
            var result = this.Controller.NotFound(tempId);
            var outcome = result.Model;

            //Assert
            Assert.IsNotNull(result);
            Assert.That(outcome, Is.EqualTo(tempId));
        }


    }
}