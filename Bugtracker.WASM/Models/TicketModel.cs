using System.ComponentModel.DataAnnotations;

namespace Bugtracker.WASM.Models
{
    public class TicketModel
    {
        public int IdTicket { get; set; }
        [MaxLength(250)]
        public string Title { get; set; }
        [Range(1, 4)]
        public int Status { get; set; }
        [Range(1, 3)]
        public int Priority { get; set; }
        [MaxLength(250)]
        public string Type { get; set; }
        [MaxLength(750)]
        public string Description { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime SubmitTime { get; set; }
        public int SubmitMember { get; set; }
        public int? AssignedMember { get; set; }
        public int Project { get; set; }
    }
    public class TicketFormModel
    {
        public int IdTicket { get; set; }
        [Required]
        [MaxLength(250)]
        public string Title { get; set; }
        [Required]
        [Range(1, 4)]
        public int Status { get; set; }
        [Required]
        [Range(1, 3)]
        public int Priority { get; set; }
        [Required]
        [MaxLength(250)]
        public string Type { get; set; }
        [Required]
        [MaxLength(750)]
        public string Description { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime SubmitTime { get; set; }
        public int? AssignedMember { get; set; }
        [Required]
        // Accept every int except zero because my value is zero if I select the first option
        [RegularExpression("^\\d*[1-9]\\d*$", ErrorMessage = "Please, choose a project.")]
        public int Project { get; set; }
    }
    public class TicketEditModel
    {
        public int IdTicket { get; set; }
        [Required]
        [MaxLength(250)]
        public string Title { get; set; }
        [Required]
        [Range(1, 4)]
        public int Status { get; set; }
        [Required]
        [Range(1, 3)]
        public int Priority { get; set; }
        [Required]
        [MaxLength(250)]
        public string Type { get; set; }
        [Required]
        [MaxLength(750)]
        public string Description { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime SubmitTime { get; set; }
        public int SubmitMember { get; set; }
        public int? AssignedMember { get; set; }
        [Required]
        public int Project { get; set; }
    }
}
