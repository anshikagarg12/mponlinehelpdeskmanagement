using System.Net.Http.Json;
using System.Text.Json;
using HelpDesk.Mvc.Models;

namespace HelpDesk.Mvc.Services
{
    /// <summary>
    /// Consumes the HelpDesk.Api TicketController endpoints using HttpClient.
    /// This is the only place in the MVC app that talks to the Web API.
    /// Direct database access is not used anywhere in the MVC project.
    /// </summary>
    public class TicketService : ITicketService
    {
        private readonly HttpClient _httpClient;

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public TicketService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Ticket>> GetAllAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/Ticket/All");
                if (!response.IsSuccessStatusCode)
                {
                    return new List<Ticket>();
                }

                var tickets = await response.Content.ReadFromJsonAsync<List<Ticket>>(JsonOptions);
                return tickets ?? new List<Ticket>();
            }
            catch (HttpRequestException)
            {
                return new List<Ticket>();
            }
        }

        public async Task<Ticket?> GetByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/Ticket/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadFromJsonAsync<Ticket>(JsonOptions);
        }

        public async Task<bool> CreateAsync(Ticket ticket)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Ticket", ticket);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAsync(Ticket ticket)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/Ticket/{ticket.Id}", ticket);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/Ticket/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<List<Ticket>> GetByStatusAsync(string status)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/Ticket/Status/{Uri.EscapeDataString(status)}");
                if (!response.IsSuccessStatusCode)
                {
                    return new List<Ticket>();
                }

                var tickets = await response.Content.ReadFromJsonAsync<List<Ticket>>(JsonOptions);
                return tickets ?? new List<Ticket>();
            }
            catch (HttpRequestException)
            {
                return new List<Ticket>();
            }
        }
    }
}
