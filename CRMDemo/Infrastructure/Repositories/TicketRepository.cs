using Application.DTO;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class TicketRepository : ITicket
    {
        private readonly ApplicationDbContext _dbContext;
        public TicketRepository(ApplicationDbContext context)
        {
            _dbContext = context;
        }
        // Retrieving tickets
        public List<Ticket> GetAllTickets(TicketFilterDTO filter)
        {

            IQueryable<Ticket> query = _dbContext.Tickets.Include(t => t.Customer);

            if (!string.IsNullOrEmpty(filter.SearchTerm))
            {
                query = query.Where(t => t.Subject.Contains(filter.SearchTerm) || t.Description.Contains(filter.SearchTerm));
            }
            if (filter.Status.HasValue)
            {
                query = query.Where(t => t.Status == filter.Status.Value);
            }
            if (filter.CustomerId > 0)
            {
                query = query.Where(t => t.CustomerId == filter.CustomerId);
            }
            return query.ToList();
        }

        public Ticket? GetTicketById(int id)
        {
            return _dbContext.Tickets.Include(t => t.Customer).FirstOrDefault(t => t.Id == id);
        }

        public void CreateTicket(TicketCreateDTO ticketDTO)
        {
            Ticket ticket = new()
            {
                Subject = ticketDTO.Subject,
                Description = ticketDTO.Description,
                CustomerId = ticketDTO.CustomerId,
                CreatedAt = DateTime.Now,
                CreatedById = 1
            };
            _dbContext.Tickets.Add(ticket);
            _dbContext.SaveChanges();
        }

        public void UpdateTicket(int id, TicketUpdateDTO ticketDTO)
        {
            var ticket = _dbContext.Tickets.Find(id);
            if (ticket == null) return;

            ticket.Subject = ticketDTO.Subject;
            ticket.Description = ticketDTO.Description;
            ticket.CustomerId = ticketDTO.CustomerId;
            ticket.Status = ticketDTO.Status;
            ticket.UpdatedAt = DateTime.Now;
            _dbContext.SaveChanges();
        }
    }
}