using HelpDesk.Api.Controllers;
using HelpDesk.Api.Models;
using HelpDesk.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HelpDesk.Tests
{
    /// <summary>
    /// Unit tests for <see cref="TicketController"/>. The repository layer is mocked
    /// with Moq so that no test connects to SQL Server.
    /// </summary>
    public class TicketControllerTests
    {
        private readonly Mock<ITicketRepository> _repository;
        private readonly TicketController _controller;

        public TicketControllerTests()
        {
            _repository = new Mock<ITicketRepository>();
            _controller = new TicketController(_repository.Object);
        }

        private static Ticket SampleTicket(int id = 1, string status = "Open") => new()
        {
            Id = id,
            Title = "Printer not working",
            Description = "The office printer is jammed.",
            Priority = "High",
            Status = status,
            RaisedBy = "john.doe",
            CreatedDate = DateTime.Now
        };

        // 1
        [Fact]
        public async Task GetAllTickets_ReturnsOkResult_WhenTicketsExist()
        {
            var tickets = new List<Ticket> { SampleTicket(1), SampleTicket(2) };
            _repository.Setup(r => r.GetAllTicketsAsync()).ReturnsAsync(tickets);

            var result = await _controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returned = Assert.IsAssignableFrom<IEnumerable<Ticket>>(okResult.Value);
            Assert.Equal(2, returned.Count());
        }

        // 2
        [Fact]
        public async Task GetTicketById_ReturnsOkResult_WhenTicketExists()
        {
            _repository.Setup(r => r.GetTicketByIdAsync(1)).ReturnsAsync(SampleTicket(1));

            var result = await _controller.GetById(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var ticket = Assert.IsType<Ticket>(okResult.Value);
            Assert.Equal(1, ticket.Id);
        }

        // 3
        [Fact]
        public async Task GetTicketById_ReturnsNotFound_WhenTicketDoesNotExist()
        {
            _repository.Setup(r => r.GetTicketByIdAsync(It.IsAny<int>())).ReturnsAsync((Ticket?)null);

            var result = await _controller.GetById(99);

            Assert.IsType<NotFoundResult>(result);
        }

        // 4
        [Fact]
        public async Task CreateTicket_ReturnsOkResult_WhenTicketIsCreatedSuccessfully()
        {
            var ticket = SampleTicket(0);
            _repository.Setup(r => r.CreateTicketAsync(It.IsAny<Ticket>())).ReturnsAsync(5);

            var result = await _controller.Create(ticket);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var created = Assert.IsType<Ticket>(okResult.Value);
            Assert.Equal(5, created.Id);
            _repository.Verify(r => r.CreateTicketAsync(It.IsAny<Ticket>()), Times.Once);
        }

        // 5
        [Fact]
        public async Task CreateTicket_ReturnsBadRequest_WhenTicketIsNull()
        {
            var result = await _controller.Create(null!);

            Assert.IsType<BadRequestResult>(result);
            _repository.Verify(r => r.CreateTicketAsync(It.IsAny<Ticket>()), Times.Never);
        }

        // 6
        [Fact]
        public async Task GetTicketsByStatus_ReturnsOkResult_WhenMatchingTicketsExist()
        {
            var tickets = new List<Ticket> { SampleTicket(1, "Open"), SampleTicket(2, "Open") };
            _repository.Setup(r => r.GetTicketsByStatusAsync("Open")).ReturnsAsync(tickets);

            var result = await _controller.GetByStatus("Open");

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returned = Assert.IsAssignableFrom<IEnumerable<Ticket>>(okResult.Value);
            Assert.Equal(2, returned.Count());
        }

        // ---------- Optional test cases ----------

        // 7
        [Fact]
        public async Task UpdateTicket_ReturnsOkResult_WhenUpdateIsSuccessful()
        {
            var ticket = SampleTicket(1);
            _repository.Setup(r => r.GetTicketByIdAsync(1)).ReturnsAsync(ticket);
            _repository.Setup(r => r.UpdateTicketAsync(It.IsAny<Ticket>())).Returns(Task.CompletedTask);

            var result = await _controller.Update(1, ticket);

            Assert.IsType<OkObjectResult>(result);
            _repository.Verify(r => r.UpdateTicketAsync(It.IsAny<Ticket>()), Times.Once);
        }

        // 8
        [Fact]
        public async Task UpdateTicket_ReturnsNotFound_WhenTicketDoesNotExist()
        {
            _repository.Setup(r => r.GetTicketByIdAsync(It.IsAny<int>())).ReturnsAsync((Ticket?)null);

            var result = await _controller.Update(99, SampleTicket(99));

            Assert.IsType<NotFoundResult>(result);
            _repository.Verify(r => r.UpdateTicketAsync(It.IsAny<Ticket>()), Times.Never);
        }

        // 9
        [Fact]
        public async Task DeleteTicket_ReturnsOkResult_WhenTicketIsDeletedSuccessfully()
        {
            _repository.Setup(r => r.GetTicketByIdAsync(1)).ReturnsAsync(SampleTicket(1));
            _repository.Setup(r => r.DeleteTicketAsync(1)).Returns(Task.CompletedTask);

            var result = await _controller.Delete(1);

            Assert.IsType<OkResult>(result);
            _repository.Verify(r => r.DeleteTicketAsync(1), Times.Once);
        }

        // 10
        [Fact]
        public async Task DeleteTicket_ReturnsNotFound_WhenTicketDoesNotExist()
        {
            _repository.Setup(r => r.GetTicketByIdAsync(It.IsAny<int>())).ReturnsAsync((Ticket?)null);

            var result = await _controller.Delete(99);

            Assert.IsType<NotFoundResult>(result);
            _repository.Verify(r => r.DeleteTicketAsync(It.IsAny<int>()), Times.Never);
        }

        // 11
        [Fact]
        public async Task GetAllTickets_ReturnsEmptyList_WhenNoTicketsExist()
        {
            _repository.Setup(r => r.GetAllTicketsAsync()).ReturnsAsync(new List<Ticket>());

            var result = await _controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returned = Assert.IsAssignableFrom<IEnumerable<Ticket>>(okResult.Value);
            Assert.Empty(returned);
        }

        // 12
        [Fact]
        public async Task GetTicketsByStatus_ReturnsEmptyList_WhenNoMatchingTicketsExist()
        {
            _repository.Setup(r => r.GetTicketsByStatusAsync(It.IsAny<string>())).ReturnsAsync(new List<Ticket>());

            var result = await _controller.GetByStatus("Closed");

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returned = Assert.IsAssignableFrom<IEnumerable<Ticket>>(okResult.Value);
            Assert.Empty(returned);
        }
    }
}
