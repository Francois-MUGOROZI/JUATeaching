using Application.DTO;
using Application.Interfaces;
using Domain.Entities;

namespace Application.Services.Tickets
{
    public class TicketService : ITicketService
    {
        private readonly ITicket _ticket;

        public TicketService(ITicket ticket)
        {
            _ticket = ticket;
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
        }

        public void UpdateTicket(int id, TicketUpdateDTO ticketDTO)
        {
            _ticket.UpdateTicket(id, ticketDTO);
        }

        public List<StatusCountDTO> GetTicketCountsByStatus()
        {
            return _ticket.GetTicketCountsByStatus();
        }
    }
}
