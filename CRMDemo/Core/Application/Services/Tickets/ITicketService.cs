using Application.DTO;
using Domain.Entities;

namespace Application.Services.Tickets
{
    public interface ITicketService
    {
        List<Ticket> GetAllTickets();
        Ticket? GetTicketById(int id);
        void CreateTicket(TicketCreateDTO ticketDTO);
        void UpdateTicket(int id, TicketUpdateDTO ticketDTO);
    }
}
