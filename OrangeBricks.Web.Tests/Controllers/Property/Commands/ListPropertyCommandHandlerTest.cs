using System.Data.Entity;
using NSubstitute;
using NUnit.Framework;
using OrangeBricks.Web.Controllers.Property.Commands;
using OrangeBricks.Web.Models;

namespace OrangeBricks.Web.Tests.Controllers.Property.Commands
{
    [TestFixture]
    public class ListPropertyCommandHandlerTest
    {
        private ListPropertyCommandHandler _handler;
        private IOrangeBricksContext _context;
        private IDbSet<Models.Property> _properties;

        [SetUp]
        public void SetUp()
        {
            _context = Substitute.For<IOrangeBricksContext>();
            _properties = Substitute.For<IDbSet<Models.Property>>();
            _context.Properties.Returns(_properties);
            _handler = new ListPropertyCommandHandler(_context);
        }

        /// <summary>
        /// GIVEN the property is not listed for sale
        /// WHEN the property is listed for sale
        /// THEN the property is listed for sale
        /// AND the changes are saved
        /// </summary>
        /// <remarks>
        /// There is no story or behaviour given - without this how do we know the behaviour being tested matches the expectations of the business?
        /// Is the assumption that all developers and testers have complete knowledge of the problem domain?
        /// I have reverse engineered the behaviour, as it is straightforward in this case; however, in general behaviours should be confirmed correct with the business analyst.
        /// </remarks>
        [Test]
        public void HandleShouldUpdatePropertyToBeListedForSale()
        {
            // Arrange
            var command = new ListPropertyCommand
            {
                PropertyId = 1
            };

            var property = new Models.Property
            {
                Description = "Test Property",
                IsListedForSale = false
            };

            _properties.Find(1).Returns(property);

            // Act
            // WHEN the property is listed for sale
            _handler.Handle(command);

            // Assert
            _context.Properties.Received(1).Find(1);
            // AND the changes are saved
            _context.Received(1).SaveChanges();
            // THEN the property is listed for sale
            Assert.True(property.IsListedForSale);
        }

        /// <summary>
        /// GIVEN the property is not valid
        /// WHEN the property is listed for sale
        /// THEN an error occurs (BEHAVIOUR TO BE DEFINED)
        /// </summary>
        /// <remarks>
        /// TODO: Failing test
        /// </remarks>
        [Test]
        public void HandleShouldUpdatePropertyToBeListedForSaleInvalidProperty()
        {
            // Arrange
            var command = new ListPropertyCommand
            {
                PropertyId = 2
            };

            var property = new Models.Property
            {
                Description = "Test Property",
                IsListedForSale = false
            };

            _properties.Find(1).Returns(property);
            _properties.Find(2).Returns(null as Models.Property);

            // Act
            _handler.Handle(command);

            // Assert
            Assert.True(property.IsListedForSale);
        }
    }
}
