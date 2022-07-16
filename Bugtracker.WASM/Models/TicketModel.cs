using System.ComponentModel.DataAnnotations;

namespace Bugtracker.WASM.Models
{
    public class TicketModel
    {
        public int IdTicket { get; set; }
        [MaxLength(250)]
        public string Title { get; set; }
        public int Status { get; set; }
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
        public int Status { get; set; }
        [Required]
        public int Priority { get; set; }
        [Required]
        [MaxLength(250)]
        public string Type { get; set; }
        [Required]
        [MaxLength(750)]
        public string Description { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime SubmitTime { get; set; }
        [Required]
        public int SubmitMember { get; set; }
        public int? AssignedMember { get; set; }
        [Required]
        public int Project { get; set; }
    }
}
