using System.ComponentModel.DataAnnotations;

namespace Bugtracker.WASM.Models
{
    public class ProjectModel
    {
        public int IdProject { get; set; }
        [MaxLength(250)]
        public string Name { get; set; }
        [MaxLength(750)]
        public string Description { get; set; }
        public int Manager { get; set; }
    }
    public class ProjectAddModel
    {
        public int IdProject { get; set; }
        [Required]
        [MaxLength(250)]
        public string Name { get; set; }
        [Required]
        [MaxLength(750)]
        public string Description { get; set; }
        [Required]
        public int Manager { get; set; }
    }
}
