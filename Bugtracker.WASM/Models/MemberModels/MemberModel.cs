using System.ComponentModel.DataAnnotations;

namespace Bugtracker.WASM.Models.MemberModels
{
    public class MemberModel
    {
        public int IdMember { get; set; }
        // TODO: Imposer un nombre minimum de chars pour le Pseudo
        [Required]
        [MaxLength(50)]
        public string Pseudo { get; set; }
        // TODO: Imposer une adresse mail dans le formulaire (pas sans @gmail.com)
        [Required]
        [MaxLength(250)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [MaxLength(50)]
        public string? Firstname { get; set; }
        [MaxLength(50)]
        public string? Lastname { get; set; }
        public bool Disabled { get; set; }
    }
    public class MemberLoginModel
    {
        [Required]
        [MaxLength(50)]
        public string Pseudo { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
