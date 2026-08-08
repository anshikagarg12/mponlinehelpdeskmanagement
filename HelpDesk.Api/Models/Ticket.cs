namespace HelpDesk.Api.Models
{
    /// <summary>
    /// Represents a support request (ticket) raised by an employee.
    /// This is the primary business model shared across the API, MVC and Test projects.
    /// </summary>
    public class Ticket
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        // Valid values: Low, Medium, High
        public string Priority { get; set; } = string.Empty;

        // Valid values: Open, In Progress, Closed
        public string Status { get; set; } = string.Empty;

        public string RaisedBy { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; }
    }
}
