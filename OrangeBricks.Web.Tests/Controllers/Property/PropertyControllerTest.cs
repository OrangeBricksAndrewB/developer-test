using System;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity;
using System.Linq;
using System.Web.Routing;
using NSubstitute;
using NUnit.Framework;
using OrangeBricks.Web.Controllers.Property;
using OrangeBricks.Web.Models;
using OrangeBricks.Web.Controllers.Property.Builders;
using OrangeBricks.Web.Controllers.Property.Commands;
using OrangeBricks.Web.Controllers.Property.ViewModels;

namespace OrangeBricks.Web.Tests.Controllers.Property
{
    [TestFixture]
    public class PropertyControllerTest
    {
        private static string TEST_URL = "http://localhost:51527";

        private PropertyController _controller;
        private IOrangeBricksContext _context;
        private IDbSet<Models.Property> _properties;
        
        [SetUp]
        public void SetUp()
        {
            _properties = Substitute.For<IDbSet<Models.Property>>();
            // wire up the IQueryable implementation for the fake data
            // Note: I see that you have implemented this in a slightly more elegant way in PropertiesViewModelBuilderTest
            // this can be refactored to follow that pattern...
            _properties.Provider.Returns(dataProperty.Provider);
            _properties.Expression.Returns(dataProperty.Expression);
            _properties.ElementType.Returns(dataProperty.ElementType);
            _properties.GetEnumerator().Returns(dataProperty.GetEnumerator());
            _properties.Find(1).Returns(dataProperty.ElementAt(0));
            _properties.Find(2).Returns(dataProperty.ElementAt(1));
            _properties.Find(3).Returns(dataProperty.ElementAt(2));

            _context = Substitute.For<IOrangeBricksContext>();
            _context.Properties.Returns(_properties);
            _controller = CreateController(_context);
        }

        /// <summary>
        /// Sanity check our fake data is setup correctly
        /// </summary>
        [Test]
        public void TestFakeData()
        {
            Assert.True(_context.Properties.Count() == 3);
            Models.Property property = _context.Properties.First();
            Assert.True(property.Equals(dataProperty.First()));
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

            Assert.True(_properties.Find(3).IsListedForSale);
        }

        /// <summary>
        /// Some fake data
        /// </summary>
        private IQueryable<Models.Property> dataProperty = new List<Models.Property>
        {
            new Models.Property{ Id= 1, PropertyType = "House", StreetName = "My Street", Description = "Great location", NumberOfBedrooms = 1, IsListedForSale = true, SellerUserId = "Seller1" },
            new Models.Property{ Id= 2, PropertyType = "Flat", StreetName = "Another Street", Description = "Rough location", NumberOfBedrooms = 2, IsListedForSale = true, SellerUserId = "Seller1" },
            new Models.Property{ Id= 3, PropertyType = "Maisonette", StreetName = "Funny Street", Description = "Good views", NumberOfBedrooms = 3, IsListedForSale = false, SellerUserId = "Seller2" }
        }.AsQueryable();

        /// <summary>
        /// Setup a fake controller context
        /// </summary>
        /// <param name="context">Data context</param>
        /// <returns>A PropertyController with a fake http context</returns>
        private PropertyController CreateController(IOrangeBricksContext context)
        {
            var browser = Substitute.For<HttpBrowserCapabilitiesBase>();
            var httpRequest = Substitute.For<HttpRequestBase>();
            httpRequest.Browser.Returns(browser);
            httpRequest.Url.Returns(new Uri(TEST_URL));

            NameValueCollection ServerVariables = new NameValueCollection {
                { "HTTP_X_FORWARDED_FOR", string.Empty},
                { "REMOTE_ADDR", string.Empty}
            };

            httpRequest.ServerVariables.Returns(ServerVariables);

            var httpContext = Substitute.For<HttpContextBase>();
            httpContext.Request.Returns(httpRequest);

            PropertyController controller = new PropertyController(context);
            controller.ControllerContext = new ControllerContext(httpContext, new RouteData(), controller);

            return controller;
        }
    }
}
