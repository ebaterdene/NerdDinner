using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NerdDinner.Controllers;
using FizzWare.NBuilder;

namespace NerdDinner.Tests.Controllers
{

    [TestFixture]
    class PaginatedListUnitTests
    {
        [SetUp]
        public void BeforeEachTestRuns()
        {
            
        }

        //private NerdDinner.Models.Dinner


        [Test]
        [TestCase(10, 0)]
        [TestCase(10, 1)]
        [TestCase(10, 11)]
        [TestCase(10, 14)]
        [TestCase(20, 4)]
        [TestCase(20, 5)]
        [TestCase(20, 6)]
        [TestCase(null, null)]
        public void PaginatedListTest(int pageSize, int pageIndex)
        {
            // Arrange
            const int sourceSize = 105;

            var dinners = Builder<NerdDinner.Models.Dinner>
                .CreateListOfSize(sourceSize)
                .Build()
                .AsQueryable()
                ;

            // Act
            var paginatedList = new PaginatedList<NerdDinner.Models.Dinner>
                    (dinners, pageIndex, pageSize);

            
            // Assert
            Assert.NotNull(paginatedList);

            if (paginatedList.Count > 0)
            {
                var dinnerStartingIndex = paginatedList.ToList().FirstOrDefault().DinnerID;
                var dinnerEndingIndex = paginatedList.ToList().Last().DinnerID + 1;
                var dinnersReturnedCount = dinnerEndingIndex - dinnerStartingIndex;
                Assert.That(dinnersReturnedCount, Is.EqualTo(paginatedList.Count));
            }
        }

        [Test]
        [TestCase(10, 0)]
        [TestCase(10, 1)]
        [TestCase(10, 11)]
        [TestCase(10, 14)]
        [TestCase(20, 4)]
        [TestCase(20, 5)]
        [TestCase(20, 6)]
        [TestCase(null, null)]
        public void HasPreviousPage(int pageSize, int pageIndex)
        {
            // Arrange
            const int sourceSize = 105;

            var dinners = Builder<NerdDinner.Models.Dinner>
                .CreateListOfSize(sourceSize)
                .Build()
                .AsQueryable()
                ;

            var paginatedList = new PaginatedList<NerdDinner.Models.Dinner>
                    (dinners, pageIndex, pageSize);


            // Assert
            Assert.NotNull(paginatedList);
            if (paginatedList.Count > 0)
            {
                Assert.That(paginatedList.PageIndex, Is.EqualTo(pageIndex));
                if (pageIndex > 0)
                {
                    Assert.That(paginatedList.HasPreviousPage, Is.True);
                }
            }
            
        }

        [Test]
        [TestCase(10, 0)]
        [TestCase(10, 1)]
        [TestCase(10, 11)]
        [TestCase(10, 14)]
        [TestCase(20, 4)]
        [TestCase(20, 5)]
        [TestCase(20, 6)]
        [TestCase(null, null)]
        public void HasNextPage(int pageSize, int pageIndex)
        {
            // Arrange
            const int sourceSize = 105;

            var dinners = Builder<NerdDinner.Models.Dinner>
                .CreateListOfSize(sourceSize)
                .Build()
                .AsQueryable()
                ;

            var paginatedList = new PaginatedList<NerdDinner.Models.Dinner>
                    (dinners, pageIndex, pageSize);
            var totalPages = (int)Math.Ceiling(dinners.Count() / (double)pageSize);

            // Assert
            Assert.NotNull(paginatedList);
            if (paginatedList.Count > 0)
            {
                Assert.That(paginatedList.PageIndex, Is.EqualTo(pageIndex));
                Assert.That(paginatedList.Totalpages, Is.EqualTo(totalPages));
                if ((pageIndex + 1) < totalPages)
                {
                    Assert.That(paginatedList.HasNextPage, Is.True);
                }
            }

        }

    }
}
