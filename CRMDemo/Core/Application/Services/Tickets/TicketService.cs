using Application.DTO;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Application.Services.Tickets
{
    public class TicketService : ITicketService
    {
        private readonly ITicket _ticket;
        private readonly ILogger<TicketService> _logger;

        public TicketService(ITicket ticket, ILogger<TicketService> logger)
        {
            _ticket = ticket;
            _logger = logger;
        }

        public List<Ticket> GetAllTickets(TicketFilterDTO filter)
        {
            return _ticket.GetAllTickets(filter);
        }

        public Ticket? GetTicketById(int id)
        {
            return _ticket.GetTicketById(id);
        }

        public void CreateTicket(TicketCreateDTO ticketDTO)
        {
            _ticket.CreateTicket(ticketDTO);
            _logger.LogInformation("Ticket created: {Subject} for customer {CustomerId}", ticketDTO.Subject, ticketDTO.CustomerId);
        }

        public void UpdateTicket(int id, TicketUpdateDTO ticketDTO)
        {
            _ticket.UpdateTicket(id, ticketDTO);
            _logger.LogInformation("Ticket updated: {TicketId}", id);
        }

        public List<StatusCountDTO> GetTicketCountsByStatus()
        {
            return _ticket.GetTicketCountsByStatus();
        }
    }
}
