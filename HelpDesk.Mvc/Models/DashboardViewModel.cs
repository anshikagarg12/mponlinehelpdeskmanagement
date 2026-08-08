namespace HelpDesk.Mvc.Models
{
    /// <summary>
    /// View model for the dashboard summary cards.
    /// </summary>
    public class DashboardViewModel
    {
        public int TotalTickets { get; set; }
        public int OpenTickets { get; set; }
        public int ClosedTickets { get; set; }
        public int InProgressTickets { get; set; }

        public List<Ticket> RecentTickets { get; set; } = new();
    }
}
