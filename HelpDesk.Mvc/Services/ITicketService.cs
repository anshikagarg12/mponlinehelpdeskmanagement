using HelpDesk.Mvc.Models;

namespace HelpDesk.Mvc.Services
{
    /// <summary>
    /// Service-layer abstraction. MVC controllers talk only to this interface,
    /// which in turn consumes the Web API over HttpClient.
    /// </summary>
    public interface ITicketService
    {
        Task<List<Ticket>> GetAllAsync();

        Task<Ticket?> GetByIdAsync(int id);

        Task<bool> CreateAsync(Ticket ticket);

        Task<bool> UpdateAsync(Ticket ticket);

        Task<bool> DeleteAsync(int id);

        Task<List<Ticket>> GetByStatusAsync(string status);
    }
}
