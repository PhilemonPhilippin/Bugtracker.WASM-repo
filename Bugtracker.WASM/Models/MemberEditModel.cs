﻿using System.ComponentModel.DataAnnotations;

namespace Bugtracker.WASM.Models
{
    public class MemberEditModel
    {
        // Check si les Required sont nécessaires
        public int IdMember { get; set; }
        [Required]
        public string Login { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
    }
}
