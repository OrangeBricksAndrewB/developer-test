using System;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity;
using System.Linq;
using NSubstitute;
using NUnit.Framework;
using OrangeBricks.Web.Controllers.Offers;
using OrangeBricks.Web.Models;
using OrangeBricks.Web.Controllers.Offers.Builders;
using OrangeBricks.Web.Controllers.Offers.Commands;
using OrangeBricks.Web.Controllers.Offers.ViewModels;
using OrangeBricks.Web.Tests.Helpers;

namespace OrangeBricks.Web.Tests.Controllers.Offers.Builders
{
    [TestFixture]
    public class MyOffersViewModelBuilderTest
    {
        private IOrangeBricksContext _context;

        [SetUp]
        public void SetUp()
        {
            MockData.SetUp();
            _context = Substitute.For<IOrangeBricksContext>();
            _context.Properties.Returns(MockData.Properties);
            _context.Offers.Returns(MockData.Offers);
        }

        [Test]
        public void MyOffersBuyerHasOffers()
        {
            // Act
            var builder = new MyOffersViewModelBuilder(_context);
            var viewModel = builder.Build("Buyer1");
            // Assert
            Assert.True(viewModel.HasOffers);

        }

        [Test]
        public void MyOffersBuyerHasOfferAccepted()
        {
            // Act
            var builder = new MyOffersViewModelBuilder(_context);
            var viewModel = builder.Build("Buyer3");
            // Assert
            Assert.True(viewModel.HasOffers);
            viewModel.MyOffers.Any(o => o.Offer.Status == OfferStatus.Accepted.ToString());
        }
    }
}
