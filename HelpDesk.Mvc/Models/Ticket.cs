using System.ComponentModel.DataAnnotations;

namespace HelpDesk.Mvc.Models
{
    /// <summary>
    /// Ticket model used by the MVC application. Mirrors the API's Ticket entity
    /// and is populated from the Web API responses via the service layer.
    /// </summary>
    public class Ticket
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(2000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public string Priority { get; set; } = string.Empty;

        [Required]
        public string Status { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Raised By")]
        public string RaisedBy { get; set; } = string.Empty;

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }
    }
}
