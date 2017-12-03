using System;
using System.ComponentModel.DataAnnotations;

namespace OrangeBricks.Web.Models
{
    public class Offer
    {
        [Key]
        public int Id { get; set; }

        public int Amount { get; set; }

        public OfferStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string BuyerUserId { get; set; }

        // The FK is not in the model - we can add it but it requires a new migration
        // alternatively, we can add just the navigation property and let EF do the work
        public int PropertyId { get; set; }
        [System.ComponentModel.DataAnnotations.Schema.ForeignKey("PropertyId")]
        public virtual Property Property { get; set; }
    }
}