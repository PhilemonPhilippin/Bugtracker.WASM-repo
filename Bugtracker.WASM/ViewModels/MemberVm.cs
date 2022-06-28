using System.ComponentModel.DataAnnotations;

namespace Bugtracker.WASM.ViewModels
{
    public class MemberVm
    {
        public int IdMember { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string EmailAddress { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
    }
    public class MemberRegistrationVm
    {
        [Required]
        public string Login { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
    }
}
