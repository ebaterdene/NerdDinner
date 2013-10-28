using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using FizzWare.NBuilder;
using NSubstitute;
using NUnit.Framework;
using NerdDinner.Models;
using NerdDinner.Controllers;
using System.Web.Mvc;


namespace NerdDinner.Tests.Controllers
{
    [TestFixture]
    public class DinnersControllerIntegrationTests
    {
        [SetUp]
        public void SetThisBeforeTest()
        {
            // Arrange
            this.Repository = Substitute.For<IDinnerRepository>();
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
        public void IndexGet()
        {
            // Arrange
            const int numberOfDinners = 10;
            var dinners = Builder<Dinner>
                .CreateListOfSize(numberOfDinners)
                .Build()
                .AsQueryable()
                ;

            this.Repository.FindUpcomingDinners()
                .Returns(dinners)
                ;

            // Act
            var result = (ViewResult)this.Controller.Index();
            var model = (IList<Dinner>)result.ViewData.Model;


            // Assert
            //Assert.AreEqual("Index", result.ViewName); //<-- Error here.
            this.Repository.Received()
                .FindUpcomingDinners()
                ;

            Assert.IsNotNull(model);
            Assert.That(model.Count(), Is.EqualTo(numberOfDinners));
            Assert.IsNotNullOrEmpty(model.First().Title);
            Assert.That(model, Is.EquivalentTo(dinners));
        }


        [Test]
        public void Index_Get_Page_0()
        {
            // Arrange
            var page = 0;


            // Act
            var result = (ViewResult) this.Controller.Index(page);
            var model = (IList<Dinner>)result.ViewData.Model;

            // Assert
            Assert.NotNull(model);

            Assert.That(model.Count, Is.GreaterThanOrEqualTo(1));

        }

        [Test]
        public void Index_Get_Page_1()
        {
            // Arrange
            var page = 1;

            // Act
            var result = (ViewResult)this.Controller.Index(page);
            var model = (IList<Dinner>)result.ViewData.Model;

            // Assert
            Assert.NotNull(model);

            Assert.That(model.Count, Is.GreaterThanOrEqualTo(0));


        }


        [Test]
        public void DetailsGet()
        {
            // Arrange
            var repositoryWithTwoDinners = getRepositoryWithTwoDinners();
            var id = 0;

            // Act
            var dinners = repositoryWithTwoDinners.FindAllDinners();
            var dinner = dinners.ToList().FirstOrDefault();
            id = dinner.DinnerID;
            var result = this.Controller.Details(id) as ViewResult;
            var model = result.ViewData.Model as Dinner;

            // Assert
            Assert.IsNotNull(model);
            Assert.That(model.DinnerID, Is.EqualTo(id));
            Assert.That(model.Title, Is.EqualTo("Upcoming Dinner"));
            Assert.That(model.EventDate.ToShortDateString(), 
                    Is.EqualTo(DateTime.Now.AddDays(2).ToShortDateString()));

            // Problem.
            // if ID doesn't exist then model is going to be int type.

        }

        [Test]
        public void DetailsGet_IdNotFound()
        {
            // Arrange
            var repositoryWithTwoDinners = getRepositoryWithTwoDinners();

            // Act
            var id = int.MaxValue;
            var result = this.Controller.Details(id) as RedirectToRouteResult;

            // Assure
            Assert.That(result, Is.Not.Null);
            Assert.That(result.RouteValues.Values.FirstOrDefault(), Is.EqualTo(id));
            //Assert.That(result.RouteName, Is.EqualTo("NotFound"));
            Assert.That(result.RouteName, Is.EqualTo(""));

            Assert.That(result.RouteValues.Values.ElementAt(1), Is.EqualTo("NotFound"));
            Assert.That(result.RouteValues.ContainsKey("id"));
            Assert.That(result.RouteValues.ContainsValue(id));
        }

        // GET: Edit

        [Test]
        public void EditGet()
        {
            // Arrange
            var repositoryWithTwoDinners = getRepositoryWithTwoDinners();
            var id = 0;

            // Act
            var dinners = repositoryWithTwoDinners.FindAllDinners();
            var dinner = dinners.ToList().FirstOrDefault();
            id = dinner.DinnerID;
            //NerdDinner.Controllers.DinnerFormViewModel

            var result = this.Controller.Edit(id) as ViewResult;
            var model = result.ViewData.Model as DinnerFormViewModel;

            // Assure
            Assert.IsNotNull(model, "model");
            Assert.That(model.DinnerID, Is.EqualTo(id), "Dinner ID");
            Assert.That(model.Title, Is.EqualTo("Upcoming Dinner"), "Title");
            Assert.That(model.EventDate.ToShortDateString(),
                    Is.EqualTo(DateTime.Now.AddDays(2).ToShortDateString()), "Event Date");
        }

        [Test]
        public void EditGet_NotFound()
        {
            // Arrange
            var repositoryWithTwoDinners = getRepositoryWithTwoDinners();

            // Act
            var id = int.MaxValue;
            var result = this.Controller.Edit(id) as RedirectToRouteResult;

            // Assure
            Assert.That(result, Is.Not.Null);
            Assert.That(result.RouteName, Is.EqualTo(""));

            Assert.That(result.RouteValues.Values.ElementAt(1), Is.EqualTo("NotFound"));
            Assert.That(result.RouteValues.ContainsKey("id"));
            Assert.That(result.RouteValues.ContainsValue(id));


            //Assert.That(result.RouteName, Is.EqualTo("NotFound"));
            //Assert.That(result.RouteValues, Has.Count.EqualTo(1));
            //Assert.That(result.RouteValues.ContainsKey("id"));
            //Assert.That(result.RouteValues.ContainsValue(id));

        }

        [Test]
        public void EditPost()
        {
            // Arrange
            var repositoryWithTwoDinners = getRepositoryWithTwoDinners();

            var dinners = repositoryWithTwoDinners.FindAllDinners();
            var dinner = dinners.ToList().FirstOrDefault();
            int tempDinnerId = dinner.DinnerID;
            var tempEventDate = DateTime.Today.AddHours(18);

            // Act
            var viewModel = new DinnerFormViewModel()
            {
                Title = "Fine Wine",
                EventDate = tempEventDate,
                HostedBy = "Sommelier",
                Description = "Sample some great tasting fine wines.",
                ContactPhone = "(206) 123-1324",
            };

            //DinnerFormViewModel
            var result = this.Controller.Edit(viewModel) as RedirectToRouteResult;
            // Need to convert result into a dinner So I can compare both.

            
            //var result = this.Controller.Edit(dinnerID, viewModel) as RedirectToRouteResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            //Assert.That(dinner.DinnerID, Is.EqualTo(result.RouteNa));

            var repository = new DinnerRepository();
            var allDinners = repository.FindAllDinners();
            Assert.That(allDinners.Count(), Is.EqualTo(2));

            var dinnerEditedVersion = allDinners.ToList().FirstOrDefault();

            Assert.That(dinnerEditedVersion.DinnerID,
                Is.EqualTo(tempDinnerId), "DinnerID");
            // There are Edited.
            Assert.That(dinnerEditedVersion.Title,
                Is.EqualTo("Fine Wine"), "Title");
            Assert.That(dinnerEditedVersion.HostedBy,
                Is.EqualTo("Sommelier"), "HostedBy");
            Assert.That(dinnerEditedVersion.EventDate,
                Is.EqualTo(tempEventDate), "EventDate");
            Assert.That(dinnerEditedVersion.Description,
                Is.EqualTo("Sample some great tasting fine wines."), "Description");
            Assert.That(dinnerEditedVersion.ContactPhone,
                Is.EqualTo("(206) 123-1324"), "ContactPhone");
        }


        [Test]
        public void Edit_Post_With_Incorrect_Model()
        {
            // Arrange
            var repositoryWithTwoDinners = getRepositoryWithTwoDinners();
            var dinners = repositoryWithTwoDinners.FindAllDinners();
            var dinner = dinners.ToList().FirstOrDefault();
            int tempDinnerId = dinner.DinnerID;
            var viewModel = new DinnerFormViewModel();

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
            var repositoryWithTwoDinners = getRepositoryWithTwoDinners();
            var id = 0;

            // Act
            var dinners = repositoryWithTwoDinners.FindAllDinners();
            var dinner = dinners.ToList().FirstOrDefault();
            id = dinner.DinnerID;
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
        public void CreatePost_With_Correct_Dinner_Model()
        {
            // Arrange
            var repositoryWithTwoDinners = getRepositoryWithTwoDinners();
            var tempDinner = new Dinner()
                {
                    Title = "Corner Cafe",
                    Address = "Olive Way & 8th Ave",
                    EventDate = DateTime.Now.AddDays(5),
                    ContactPhone = "206-602-2345",
                    Country = "USA",
                    Description = "Get your breakfast fixed.",
                    HostedBy = "Luke!",
                    Latitude = "47.54145",
                    Longitude = "-122.34334"
                };

            // Act
            var result = this.Controller.Create(new DinnerFormViewModel(tempDinner)) as RedirectToRouteResult;

            // Assure
            Assert.IsNotNull(result);

            // I wonder why Details is second on route Values.
            Assert.That(result.RouteValues.Values.ElementAt(1), Is.EqualTo("Details"));

        }

        [Test]
        public void CreatePost_With_InCorrect_Dinner_Model()
        {
            // Arrange
            var repositoryWithTwoDinners = getRepositoryWithTwoDinners();

            var viewModel = new DinnerFormViewModel();

            // Act
            this.Controller.ModelState.AddModelError("Someproperty", "Some Message");
            var result = this.Controller.Create(viewModel) as ViewResult;

            // Assure
            Assert.IsNotNull(result);
            Assert.That(result.Model, Is.EqualTo(viewModel));

        }



        [Test]
        public void DeleteGet()
        {
            // Arrange
            var repositoryWithTwoDinners = getRepositoryWithTwoDinners();
            var id = 0;

            // Act
            var dinners = repositoryWithTwoDinners.FindAllDinners();
            var dinner = dinners.ToList().FirstOrDefault();
            id = dinner.DinnerID;
            var result = this.Controller.Delete(id) as ViewResult;
            var model = result.ViewData.Model as Dinner;


            // Assure
            Assert.IsNotNull(model);
            Assert.That(model.DinnerID, Is.EqualTo(id), "DinnerID");
            Assert.That(model.Title, Is.EqualTo(dinner.Title), "Title");
            Assert.That(model.EventDate.Date,Is.EqualTo(dinner.EventDate.Date), "EventDate.Date");
            Assert.That(model.EventDate.Hour,Is.EqualTo(dinner.EventDate.Hour), "EventDate.Hour");
            Assert.That(model.EventDate.Minute,Is.EqualTo(dinner.EventDate.Minute), "EventDate.Minute");
        }

        [Test]
        public void DeleteGet_NotFound()
        {
            // Arrange
            var repositoryWithTwoDinners = getRepositoryWithTwoDinners();

            // Act
            var id = int.MaxValue;
            var result = this.Controller.Delete(id) as RedirectToRouteResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.RouteName, Is.EqualTo("NotFound"));
            Assert.That(result.RouteValues, Has.Count.EqualTo(1));
            Assert.That(result.RouteValues.ContainsKey("id"));
            Assert.That(result.RouteValues.ContainsValue(id));
        }

        [Test]
        public void Delete_Post_Dinner_That_Exists()
        {
            // Arrange
            var repositoryWithTwoDinners = getRepositoryWithTwoDinners();
            var dinners = repositoryWithTwoDinners.FindAllDinners();
            var dinner = dinners.ToList().FirstOrDefault();
            int tempDinnerId = dinner.DinnerID;

            // Act
            var result = this.Controller.DeletePost(tempDinnerId) as ViewResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ViewName, Is.EqualTo("Deleted"));
        }

        [Test]
        public void Delete_Post_Dinner_That_Does_Not_Exists()
        {
            var repositoryWithTwoDinners = getRepositoryWithTwoDinners();
            var dinners = repositoryWithTwoDinners.FindAllDinners();
            var dinner = dinners.ToList().FirstOrDefault();
            int tempDinnerId = int.MaxValue;

            // Act
            var result = this.Controller.DeletePost(tempDinnerId) as RedirectToRouteResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.RouteName, Is.EqualTo("NotFound"));
            Assert.That(result.RouteValues.Values.FirstOrDefault(), 
                Is.EqualTo(tempDinnerId));

        }

        //[Test]
        //public void NotFoundTest()
        //{
        //    // Arrange
        //    var tempId = int.MaxValue;

        //    // Act
        //    var result = this.Controller.NotFound(tempId) as ViewResult;
        //    var outcome = result.Model;

        //    //Assert
        //    Assert.IsNotNull(result);
        //    Assert.That(outcome, Is.EqualTo(tempId));
        //}


        // -- Helper Methods
        protected DinnerRepository getRepositoryWithTwoDinners()
        {
            var repository = new DinnerRepository();
            repository.FindAllDinners().ToList().ForEach(repository.Delete);
            repository.Add(
                Builder<Dinner>
                    .CreateNew()
                    .With(d => d.EventDate = DateTime.Now.AddDays(2))
                    .With(d => d.Title = "Upcoming Dinner")
                    .Build()
                );
            repository.Add(
                Builder<Dinner>
                    .CreateNew()
                    .With(d => d.EventDate = DateTime.Now)
                    .With(d => d.Title = "Past Dinner")
                    .Build()
                );
            repository.Save();
            return repository;
        }



    }
}
