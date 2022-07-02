using System.ComponentModel.DataAnnotations;

namespace Bugtracker.WASM.Models
{
    public class MemberRegistrationModel
    {
        [Required]
        public string Pseudo { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
        
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
    }
}
