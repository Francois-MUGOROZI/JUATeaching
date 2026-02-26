using System.ComponentModel.DataAnnotations;
using Domain.ValueObjects;

namespace Application.DTO
{
    public class TicketCreateDTO
    {
        [Required(ErrorMessage = "Subject is required")]
        public string Subject { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "Please select a customer")]
        public int CustomerId { get; set; }
    }

    public class TicketUpdateDTO
    {
        [Required(ErrorMessage = "Subject is required")]
        public string Subject { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "Please select a customer")]
        public int CustomerId { get; set; }

        public TicketStatus Status { get; set; } = TicketStatus.Open;
    }

    public class TicketDetailDTO
    {
        public int Id { get; set; }
        public string Subject { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TicketStatus Status { get; set; } = TicketStatus.Open;
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }

    public class TicketFilterDTO
    {
        public string? SearchTerm { get; set; }
        public TicketStatus? Status { get; set; }
        public DateTime? CreatedFrom { get; set; }
        public DateTime? CreatedTo { get; set; }

        public int CustomerId { get; set; }
    }

    public class StatusCountDTO
    {
        public TicketStatus Status { get; set; }
        public int Count { get; set; }
    }
}