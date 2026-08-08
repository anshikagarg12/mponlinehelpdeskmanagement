using HelpDesk.Mvc.Models;
using HelpDesk.Mvc.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HelpDesk.Mvc.Controllers
{
    public class TicketsController : Controller
    {
        private readonly ITicketService _ticketService;

        public TicketsController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        // GET: /Tickets  -> View All Tickets
        public async Task<IActionResult> Index()
        {
            var tickets = await _ticketService.GetAllAsync();
            return View(tickets);
        }

        // GET: /Tickets/Details/5  -> View Ticket Details
        public async Task<IActionResult> Details(int id)
        {
            var ticket = await _ticketService.GetByIdAsync(id);
            if (ticket is null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // GET: /Tickets/Create  -> Raise New Ticket
        public IActionResult Create()
        {
            PopulatePriorities();
            // Status is hard-coded to "Open" for new tickets.
            var ticket = new Ticket { Status = "Open" };
            return View(ticket);
        }

        // POST: /Tickets/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Ticket ticket)
        {
            // Status is always "Open" when a ticket is raised.
            ticket.Status = "Open";
            ticket.CreatedDate = DateTime.Now;

            if (!ModelState.IsValid)
            {
                PopulatePriorities(ticket.Priority);
                return View(ticket);
            }

            var success = await _ticketService.CreateAsync(ticket);
            if (!success)
            {
                ModelState.AddModelError(string.Empty, "Unable to create the ticket. Please try again.");
                PopulatePriorities(ticket.Priority);
                return View(ticket);
            }

            TempData["Message"] = "Ticket raised successfully.";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Tickets/Edit/5  -> Edit Ticket
        public async Task<IActionResult> Edit(int id)
        {
            var ticket = await _ticketService.GetByIdAsync(id);
            if (ticket is null)
            {
                return NotFound();
            }

            PopulatePriorities(ticket.Priority);
            PopulateStatuses(ticket.Status);
            return View(ticket);
        }

        // POST: /Tickets/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Ticket ticket)
        {
            if (!ModelState.IsValid)
            {
                PopulatePriorities(ticket.Priority);
                PopulateStatuses(ticket.Status);
                return View(ticket);
            }

            var success = await _ticketService.UpdateAsync(ticket);
            if (!success)
            {
                ModelState.AddModelError(string.Empty, "Unable to update the ticket. Please try again.");
                PopulatePriorities(ticket.Priority);
                PopulateStatuses(ticket.Status);
                return View(ticket);
            }

            TempData["Message"] = "Ticket updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Tickets/Delete/5  -> confirm delete
        public async Task<IActionResult> Delete(int id)
        {
            var ticket = await _ticketService.GetByIdAsync(id);
            if (ticket is null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: /Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _ticketService.DeleteAsync(id);
            TempData["Message"] = "Ticket deleted successfully.";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Tickets/Filter  -> Filter Tickets by Status
        public async Task<IActionResult> Filter(string? status)
        {
            PopulateStatuses(status);

            var tickets = string.IsNullOrEmpty(status)
                ? new List<Ticket>()
                : await _ticketService.GetByStatusAsync(status);

            ViewBag.SelectedStatus = status;
            return View(tickets);
        }

        private void PopulatePriorities(string? selected = null)
        {
            ViewBag.Priorities = new SelectList(TicketOptions.Priorities, selected);
        }

        private void PopulateStatuses(string? selected = null)
        {
            ViewBag.Statuses = new SelectList(TicketOptions.Statuses, selected);
        }
    }
}
