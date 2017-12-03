using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Data.Entity;
using NSubstitute;
using NUnit.Framework;
using OrangeBricks.Web.Models;
using System.Web.Mvc;
using System.Web;
using System.Web.Routing;

namespace OrangeBricks.Web.Tests.Helpers
{
    public static class MockData
    {
        public static string TEST_URL = "http://localhost:51527";

        public static IDbSet<Models.Property> Properties;
        public static IDbSet<Models.Offer> Offers;

        /// <summary>
        /// wire up the IQueryable implementation for mocked entity data
        /// </summary>
        /// <typeparam name="T">Type of the entity data</typeparam>
        /// <param name="dbSet">Mocked DbSet<entity data type></param>
        /// <param name="data">The data to mock as IQueryable</param>
        /// <returns></returns>
        public static IDbSet<T> Initialize<T>(this IDbSet<T> dbSet, IQueryable<T> data) where T : class
        {
            dbSet.Provider.Returns(data.Provider);
            dbSet.Expression.Returns(data.Expression);
            dbSet.ElementType.Returns(data.ElementType);
            dbSet.GetEnumerator().Returns(data.GetEnumerator());
            return dbSet;
        }

        /// <summary>
        /// wire up the IQueryable implementation for the mock data
        /// (not mocking the Offers navigation property)
        /// </summary>
        public static void SetUp()
        {
            Properties = Substitute.For<IDbSet<Models.Property>>().Initialize(dataProperty.AsQueryable());
            Properties.Find(1).Returns(MockData.dataProperty.ElementAt(0));
            Properties.Find(2).Returns(MockData.dataProperty.ElementAt(1));
            Properties.Find(3).Returns(MockData.dataProperty.ElementAt(2));

            Offers = Substitute.For<IDbSet<Models.Offer>>().Initialize(dataOffer.AsQueryable());
            Offers.Find(1).Returns(MockData.dataOffer.ElementAt(0));
            Offers.Find(2).Returns(MockData.dataOffer.ElementAt(1));
            Offers.Find(3).Returns(MockData.dataOffer.ElementAt(2));

        }

        /// <summary>
        /// Some fake data
        /// </summary>
        public static IQueryable<Models.Property> dataProperty = new List<Models.Property>
        {
            new Models.Property{ Id= 1, PropertyType = "House", StreetName = "My Street", Description = "Great location", NumberOfBedrooms = 1, IsListedForSale = true, SellerUserId = "Seller1" },
            new Models.Property{ Id= 2, PropertyType = "Flat", StreetName = "Another Street", Description = "Rough location", NumberOfBedrooms = 2, IsListedForSale = true, SellerUserId = "Seller1" },
            new Models.Property{ Id= 3, PropertyType = "Maisonette", StreetName = "Funny Street", Description = "Good views", NumberOfBedrooms = 3, IsListedForSale = false, SellerUserId = "Seller2" }
        }.AsQueryable();

        public static IQueryable<Models.Offer> dataOffer = new List<Models.Offer>
        {
            new Models.Offer { Id = 1, Property = dataProperty.ElementAt(0), PropertyId = 1, Amount = 100000, CreatedAt = System.DateTime.Now, UpdatedAt = System.DateTime.Now, Status = OfferStatus.Pending, BuyerUserId = "Buyer1" },
            new Models.Offer { Id = 2, Property = dataProperty.ElementAt(1), PropertyId = 1, Amount = 150000, CreatedAt = System.DateTime.Now, UpdatedAt = System.DateTime.Now, Status = OfferStatus.Pending, BuyerUserId = "Buyer2" },
            new Models.Offer { Id = 3, Property = dataProperty.ElementAt(2), PropertyId = 2, Amount = 250000, CreatedAt = System.DateTime.Now, UpdatedAt = System.DateTime.Now, Status = OfferStatus.Accepted, BuyerUserId = "Buyer3" }
        }.AsQueryable();

        /// <summary>
        /// Setup a fake controller context
        /// </summary>
        /// <param name="context">Data context</param>
        /// <returns>A PropertyController with a fake http context</returns>
        public static T CreateControllerContext<T>(T controller) where T : Controller
        {
            var browser = Substitute.For<HttpBrowserCapabilitiesBase>();
            var httpRequest = Substitute.For<HttpRequestBase>();
            httpRequest.Browser.Returns(browser);
            httpRequest.Url.Returns(new Uri(TEST_URL));

            NameValueCollection ServerVariables = new NameValueCollection {
                { "PATH_INFO", string.Empty}
                // insert as needed
            };
            httpRequest.ServerVariables.Returns(ServerVariables);

            var httpContext = Substitute.For<HttpContextBase>();
            httpContext.Request.Returns(httpRequest);

            controller.ControllerContext = new ControllerContext(httpContext, new RouteData(), controller);
            return controller;
        }
    }

}
