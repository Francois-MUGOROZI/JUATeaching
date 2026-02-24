
using Domain.ValueObjects;

namespace Domain.Entities
{

    public class Ticket
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        // Status is converted to string in database using Fluent API in ApplicationDbContext (on model creating)
        public TicketStatus Status { get; set; } = TicketStatus.Open;

        // Customer Relation
        public int CustomerId { get; set; }
        public Customer Customer { get; set; } // Navigation prop (belong)

        /// Audit fields
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public int? CreatedById { get; set; }
        public int? UpdatedById { get; set; }
    }
}
