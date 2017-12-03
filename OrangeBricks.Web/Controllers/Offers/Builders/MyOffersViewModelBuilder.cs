using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using OrangeBricks.Web.Controllers.Offers.ViewModels;
using OrangeBricks.Web.Controllers.Property.ViewModels;
using OrangeBricks.Web.Models;

namespace OrangeBricks.Web.Controllers.Offers.Builders
{
    public class MyOffersViewModelBuilder
    {
        private readonly IOrangeBricksContext _context;

        public MyOffersViewModelBuilder(IOrangeBricksContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Build a MyOffersViewModel for the specified buyer
        /// </summary>
        /// <param name="buyer"></param>
        /// <returns></returns>
        public MyOffersViewModel Build(string buyer)
        {
            // get the list of offers made by the buyer
            var offers = _context.Offers.Where(offer => offer.BuyerUserId == buyer);
            var myOffers =
                offers.Select(
                    o => new MyOfferViewModel
                    {
                        Offer = new OfferViewModel()
                        {
                            Id = o.Id,
                            Amount = o.Amount,
                            CreatedAt = o.CreatedAt,
                            IsPending = o.Status == OfferStatus.Pending,
                            Status = o.Status.ToString()
                        },
                        Property = new PropertyViewModel()
                        {
                            Id = o.Property.Id,
                            StreetName = o.Property.StreetName,
                            Description = o.Property.Description,
                            NumberOfBedrooms = o.Property.NumberOfBedrooms,
                            PropertyType = o.Property.PropertyType,
                            IsListedForSale = o.Property.IsListedForSale
                        }
                    });

            return new MyOffersViewModel
            {
                BuyerUserId = buyer,
                HasOffers = myOffers.Any(),
                MyOffers = myOffers.ToList() // acceptable use of ToList
            };
        }
    }
}