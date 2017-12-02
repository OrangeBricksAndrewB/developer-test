using System;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Linq;
using OrangeBricks.Web.Controllers.Property.ViewModels;
using OrangeBricks.Web.Models;

namespace OrangeBricks.Web.Controllers.Property.Builders
{
    public class PropertiesViewModelBuilder
    {
        private readonly IOrangeBricksContext _context;

        public PropertiesViewModelBuilder(IOrangeBricksContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public PropertiesViewModel Build(PropertiesQuery query)
        {
            // Discussion point: implicit or explicit declaration?
            //var properties = _context.Properties
            IQueryable<Models.Property> properties = _context.Properties
                .Where(p => p.IsListedForSale);

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                properties = properties.Where(x => x.StreetName.Contains(query.Search) 
                    || x.Description.Contains(query.Search));
            }

            return new PropertiesViewModel
            {
                Properties = properties
                    //.ToList() redundant use of ToList
                    .Select(MapViewModel)
                    .ToList(), // TODO: undesirable use of ToList here - suppose there are a million properties...
                                // needs refactoring to return an IEnumerable so views can load properties on demand
                Search = query.Search
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        /// <remarks>Fixed BUG: IsListedForSale property not set</remarks>
        private static PropertyViewModel MapViewModel(Models.Property property)
        {
            return new PropertyViewModel
            {
                Id = property.Id,
                StreetName = property.StreetName,
                Description = property.Description,
                NumberOfBedrooms = property.NumberOfBedrooms,
                PropertyType = property.PropertyType,
                IsListedForSale = property.IsListedForSale
            };
        }
    }
}