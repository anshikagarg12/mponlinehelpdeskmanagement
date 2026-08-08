# HelpDeskManagement

A complete Help Desk Ticket Management System, built with ASP.NET Core Web API, ASP.NET Core MVC, Entity Framework Core, xUnit, and Moq.

Employees raise tickets for software, hardware, and network issues. The Help Desk team creates, reviews, updates, filters, and deletes those tickets.

## Solution Structure

| Project | Type | Purpose |
|---|---|---|
| **HelpDesk.Api** | ASP.NET Core Web API | Serves REST endpoints through Entity Framework Core, SQL Server, and the Repository Pattern |
| **HelpDesk.Mvc** | ASP.NET Core MVC | Talks to the Web API through a Service Layer (`HttpClient`) |
| **HelpDesk.Tests** | xUnit Test Project | Covers the API controller with Moq-based unit tests |

## Tech Stack

- .NET 8
- ASP.NET Core Web API + MVC
- Entity Framework Core 8 (SQL Server / LocalDB)
- Repository Pattern
- xUnit + Moq
- Bootstrap 5

## The Ticket Model

```csharp
public class Ticket
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Priority { get; set; }   // Low, Medium, High
    public string Status { get; set; }      // Open, In Progress, Closed
    public string RaisedBy { get; set; }
    public DateTime CreatedDate { get; set; }
}
```

## API Endpoints (`HelpDesk.Api`)

| HTTP Method | Endpoint | Description |
|---|---|---|
| GET | `/api/Ticket/All` | Returns every ticket |
| GET | `/api/Ticket/{id}` | Returns a ticket by Id |
| POST | `/api/Ticket` | Creates a ticket |
| PUT | `/api/Ticket/{id}` | Updates a ticket |
| DELETE | `/api/Ticket/{id}` | Deletes a ticket |
| GET | `/api/Ticket/Status/{status}` | Returns tickets matching a status |

The API applies the Repository Pattern. `ITicketRepository` and `TicketRepository` manage database access through `AppDbContext`, and the controller depends only on the repository interface.

## MVC Application (`HelpDesk.Mvc`)

MVC controllers call only the Service Layer (`ITicketService` / `TicketService`), which reaches the Web API over `HttpClient`. The MVC project never touches the database directly.

Features:

- **Dashboard** — displays Total, Open, In Progress, and Closed ticket counts
- **View All Tickets**
- **View Ticket Details**
- **Raise New Ticket** — Status starts as `Open`; pick Priority from a dropdown
- **Edit Ticket** — update Title, Description, Priority, and Status through dropdowns
- **Delete Ticket**
- **Filter Tickets by Status** — choose Open, In Progress, or Closed from a dropdown; matching tickets appear in a table

## Unit Tests (`HelpDesk.Tests`)

Moq mocks the repository layer, so the tests run without touching SQL Server. All 12 tests pass: 6 mandatory, 6 optional.

1. `GetAllTickets_ReturnsOkResult_WhenTicketsExist`
2. `GetTicketById_ReturnsOkResult_WhenTicketExists`
3. `GetTicketById_ReturnsNotFound_WhenTicketDoesNotExist`
4. `CreateTicket_ReturnsOkResult_WhenTicketIsCreatedSuccessfully`
5. `CreateTicket_ReturnsBadRequest_WhenTicketIsNull`
6. `GetTicketsByStatus_ReturnsOkResult_WhenMatchingTicketsExist`
7. `UpdateTicket_ReturnsOkResult_WhenUpdateIsSuccessful`
8. `UpdateTicket_ReturnsNotFound_WhenTicketDoesNotExist`
9. `DeleteTicket_ReturnsOkResult_WhenTicketIsDeletedSuccessfully`
10. `DeleteTicket_ReturnsNotFound_WhenTicketDoesNotExist`
11. `GetAllTickets_ReturnsEmptyList_WhenNoTicketsExist`
12. `GetTicketsByStatus_ReturnsEmptyList_WhenNoMatchingTicketsExist`

## Getting Started

### Prerequisites

- .NET 8 SDK
- SQL Server LocalDB (or update the connection string in `HelpDesk.Api/appsettings.json`)

### Database

The API runs EF Core migrations and seeds sample data on startup. To create or update the database by hand:

```bash
dotnet ef database update --project HelpDesk.Api
```

### Run

Start the API and the MVC app in two terminals:

```bash
dotnet run --project HelpDesk.Api    # http://localhost:5100
dotnet run --project HelpDesk.Mvc    # http://localhost:5200
```

Open the MVC app at **http://localhost:5200**.
`HelpDesk.Mvc/appsettings.json` (`ApiBaseUrl`) points to the API base URL the MVC app calls.

### Test

```bash
dotnet test
```

