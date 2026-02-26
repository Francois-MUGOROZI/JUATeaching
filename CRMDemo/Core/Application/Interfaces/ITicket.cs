using Application.DTO;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface ITicket
    {
        List<Ticket> GetAllTickets(TicketFilterDTO filter);
        Ticket? GetTicketById(int id);
        void CreateTicket(TicketCreateDTO ticketDTO);
        void UpdateTicket(int id, TicketUpdateDTO ticketDTO);
    }
}
