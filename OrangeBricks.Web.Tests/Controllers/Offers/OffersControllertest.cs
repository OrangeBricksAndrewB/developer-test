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

namespace OrangeBricks.Web.Tests.Controllers.Offers
{
    [TestFixture]
    public class OffersControllertest
    {
        private OffersController _controller;
        private IOrangeBricksContext _context;

        [SetUp]
        public void SetUp()
        {
            MockData.SetUp();
            _context = Substitute.For<IOrangeBricksContext>();
            _context.Properties.Returns(MockData.Properties);
            _context.Offers.Returns(MockData.Offers);

            _controller = MockData.CreateControllerContext(new OffersController(_context));
        }

        
    }
}
