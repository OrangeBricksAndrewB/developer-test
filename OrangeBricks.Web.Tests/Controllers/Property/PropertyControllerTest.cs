using System;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity;
using System.Linq;
using NSubstitute;
using NUnit.Framework;
using OrangeBricks.Web.Controllers.Property;
using OrangeBricks.Web.Models;
using OrangeBricks.Web.Controllers.Property.Builders;
using OrangeBricks.Web.Controllers.Property.Commands;
using OrangeBricks.Web.Controllers.Property.ViewModels;
using OrangeBricks.Web.Tests.Helpers;

namespace OrangeBricks.Web.Tests.Controllers.Property
{

    [TestFixture]
    public class PropertyControllerTest
    {
        private PropertyController _controller;
        private IOrangeBricksContext _context;
        
        [SetUp]
        public void SetUp()
        {
            MockData.SetUp();
            _context = Substitute.For<IOrangeBricksContext>();
            _context.Properties.Returns(MockData.Properties);
            _context.Offers.Returns(MockData.Offers);

            _controller = MockData.CreateControllerContext(new PropertyController(_context));
        }

        /// <summary>
        /// Sanity check our fake data is setup correctly
        /// </summary>
        [Test]
        public void TestFakeData()
        {
            Assert.True(_context.Properties.Count() == 3);
            Models.Property property = _context.Properties.First();
            Assert.True(property.Equals(MockData.dataProperty.First()));
        }

        [Test]
        public void Index()
        {
            //Arrange - is done by the class Setup
            // Act
            var result= _controller.Index(new PropertiesQuery());
            // Assert
            Assert.IsAssignableFrom(typeof(ViewResult), result);
            ViewResult viewResult = result as ViewResult;
            Assert.IsAssignableFrom(typeof(PropertiesViewModel), viewResult.Model);
            PropertiesViewModel model = viewResult.Model as PropertiesViewModel;
            Assert.True(model.Properties.Count() == 2); // 2 properties for sale
            Assert.False(model.Properties.Any(p => !p.IsListedForSale));

        }

        [Test]
        public void ListForSale()
        {
            var result = _controller.ListForSale(new ListPropertyCommand { PropertyId = 3 });

            Assert.IsAssignableFrom(typeof(RedirectToRouteResult), result);
            var viewResult = result as RedirectToRouteResult;
            Assert.True(viewResult.RouteValues.ContainsValue("MyProperties"));

            Assert.True(_context.Properties.Find(3).IsListedForSale);
        }

        [Test]
        public void TestOfferData()
        {
            Assert.True(_context.Offers.Count() == 3);
            Models.Offer offer = _context.Offers.First();
            Assert.True(offer.Equals(MockData.dataOffer.First()));

            // offers on a property

        }

    }
}
