
using System.ComponentModel.DataAnnotations;
using Domain.ValueObjects;

namespace Domain.Entities
{

    public class Customer
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(20), Required]
        public string FirstName { get; set; }
        [MaxLength(20), Required]
        public string LastName { get; set; }
        [MaxLength(50), Required]
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        // Status is converted to string in database using Fluent API in ApplicationDbContext (on model creating)
        public CustomerStatus Status { get; set; } = CustomerStatus.Active;

        public List<Ticket> Tickets { get; set; } // Owns tickets

        /// Audit fields
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public int? CreatedById { get; set; }
        public int? UpdatedById { get; set; }
    }
}
