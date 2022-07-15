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
}
