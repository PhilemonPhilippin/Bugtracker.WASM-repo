using System.ComponentModel.DataAnnotations;

namespace Bugtracker.WASM.Models.MemberModels
{
    public class MemberModel
    {
        public int IdMember { get; set; }
        public string Pseudo { get; set; }
        public string Email { get; set; }
        public string PswdHash { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
    }
}
