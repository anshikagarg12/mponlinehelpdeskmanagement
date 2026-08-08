using HelpDesk.Api.Models;
using HelpDesk.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace HelpDesk.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketController : ControllerBase
    {
        private readonly ITicketRepository _repository;

        public TicketController(ITicketRepository repository)
        {
            _repository = repository;
        }

        // GET: api/Ticket/All
        [HttpGet("All")]
        public async Task<IActionResult> GetAll()
        {
            var tickets = await _repository.GetAllTicketsAsync();
            return Ok(tickets);
        }

        // GET: api/Ticket/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var ticket = await _repository.GetTicketByIdAsync(id);
            if (ticket is null)
            {
                return NotFound();
            }

            return Ok(ticket);
        }

        // POST: api/Ticket
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Ticket ticket)
        {
            if (ticket is null)
            {
                return BadRequest();
            }

            var newId = await _repository.CreateTicketAsync(ticket);
            ticket.Id = newId;
            return Ok(ticket);
        }

        // PUT: api/Ticket/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] Ticket ticket)
        {
            if (ticket is null)
            {
                return BadRequest();
            }

            var existing = await _repository.GetTicketByIdAsync(id);
            if (existing is null)
            {
                return NotFound();
            }

            ticket.Id = id;
            await _repository.UpdateTicketAsync(ticket);
            return Ok(ticket);
        }

        // DELETE: api/Ticket/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _repository.GetTicketByIdAsync(id);
            if (existing is null)
            {
                return NotFound();
            }

            await _repository.DeleteTicketAsync(id);
            return Ok();
        }

        // GET: api/Ticket/Status/{status}
        [HttpGet("Status/{status}")]
        public async Task<IActionResult> GetByStatus(string status)
        {
            var tickets = await _repository.GetTicketsByStatusAsync(status);
            return Ok(tickets);
        }
    }
}
