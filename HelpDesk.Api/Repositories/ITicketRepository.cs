using HelpDesk.Api.Models;

namespace HelpDesk.Api.Repositories
{
    /// <summary>
    /// Repository abstraction for all Ticket database operations.
    /// </summary>
    public interface ITicketRepository
    {
        Task<List<Ticket>> GetAllTicketsAsync();

        Task<Ticket?> GetTicketByIdAsync(int id);

        Task<int> CreateTicketAsync(Ticket ticket);

        Task UpdateTicketAsync(Ticket ticket);

        Task DeleteTicketAsync(int id);

        Task<List<Ticket>> GetTicketsByStatusAsync(string status);
    }
}
