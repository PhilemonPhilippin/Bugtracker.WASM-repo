using System.ComponentModel.DataAnnotations;

namespace Bugtracker.WASM.Models
{
    public class MemberModel
    {
        public int IdMember { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string EmailAddress { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
    }
}
