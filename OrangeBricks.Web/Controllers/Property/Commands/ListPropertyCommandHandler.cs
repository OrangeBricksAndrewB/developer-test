using OrangeBricks.Web.Models;

namespace OrangeBricks.Web.Controllers.Property.Commands
{
    /// <summary>
    /// Please use XML comments to document your classes.
    /// Implements business logic to list a property (list relevant links to business requirements, user stories etc).
    /// </summary>
    /// <remarks>Satisfies Single Responsibility Principle.</remarks>
    public class ListPropertyCommandHandler
    {
        private readonly IOrangeBricksContext _context;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context">Database context (DbContext)</param>
        /// <remarks>Constructor injection is an acceptable technique for setting the Database context.</remarks>
        public ListPropertyCommandHandler(IOrangeBricksContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Please use XML comments to state the expected behaviour or outcome (or reference the user story/Gherkin).
        /// This is particularly important if the controller actions are merely orchestrators i.e. they do not execute the business logic.
        /// </summary>
        /// <param name="command"></param>
        /// <remarks>This handler will raise an exception if the PropertyId is not found.
        /// This *is* the behaviour; however, is it the *correct* behaviour? Has this been discussed? Is it a conscious design decision?
        /// If so, how is this intent being communicated to me? You need to state this explicitly, or provide an additional UT that exercises this behaviour.
        /// </remarks>
        public void Handle(ListPropertyCommand command)
        {
            var property = _context.Properties.Find(command.PropertyId);
            // we have not defined behaviours for the case where the property is already listed
            // it may be that idempotency is desirable, and we can implement that here and also save a write to the DB
            if (!property.IsListedForSale)
            {
                property.IsListedForSale = true;
                _context.SaveChanges();
            }
        }
    }
}