namespace HelpDesk.Mvc.Models
{
    /// <summary>
    /// Central definition of the allowed Priority and Status values,
    /// used to populate dropdowns across the MVC views.
    /// </summary>
    public static class TicketOptions
    {
        public static readonly string[] Priorities = { "Low", "Medium", "High" };

        public static readonly string[] Statuses = { "Open", "In Progress", "Closed" };
    }
}
