using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OrangeBricks.Web.Controllers.Property.ViewModels;

namespace OrangeBricks.Web.Controllers.Offers.ViewModels
{
    /// <summary>
    /// Exposes the offers made by a particular buyer
    /// </summary>
    public class MyOffersViewModel
    {
        public string BuyerUserId { get; set; } //me
        public bool HasOffers { get; set; }
        public List<MyOfferViewModel> MyOffers { get; set; }
    }

    /// <summary>
    /// Exposes an offer and the related property
    /// </summary>
    public class MyOfferViewModel
    {
        /// <summary>
        /// My offer on the Property
        /// </summary>
        public OfferViewModel Offer { get; set; }

        /// <summary>
        /// The related property
        /// </summary>
        public PropertyViewModel Property { get; set; }
    }

}