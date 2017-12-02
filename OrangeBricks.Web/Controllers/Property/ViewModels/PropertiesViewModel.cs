using System.Collections.Generic;

namespace OrangeBricks.Web.Controllers.Property.ViewModels
{
    public class PropertiesViewModel
    {
        public List<PropertyViewModel> Properties { get; set; }
        /// <summary>
        /// What is the purpose of this property?
        /// </summary>
        public string Search { get; set; }
    }
}