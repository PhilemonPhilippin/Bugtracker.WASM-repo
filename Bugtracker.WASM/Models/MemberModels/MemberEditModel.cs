using System.ComponentModel.DataAnnotations;

namespace Bugtracker.WASM.Models.MemberModels
{
    public class MemberEditModel
    {
        public int IdMember { get; set; }
        [Required]
        public string Pseudo { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
    }
}
