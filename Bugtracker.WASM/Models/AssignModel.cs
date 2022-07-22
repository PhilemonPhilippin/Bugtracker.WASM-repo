namespace Bugtracker.WASM.Models
{
    public class AssignModel
    {
        public int IdAssign { get; set; }
        public DateTime AssignTime { get; set; }
        public int Project { get; set; }
        public int Member { get; set; }
    }
    public class AssignMinimalModel
    {
        public int Project { get; set; }
        public int Member { get; set; }
    }
}
