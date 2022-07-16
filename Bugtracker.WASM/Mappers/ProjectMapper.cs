using Bugtracker.WASM.Models;

namespace Bugtracker.WASM.Mappers
{
    internal static class ProjectMapper
    {
        public static ProjectModel ToModel(this ProjectAddModel addModel)
        {
            return new ProjectModel()
            {
                IdProject = 0,
                Name = addModel.Name,
                Description = addModel.Description,
                Manager = addModel.Manager
            };
        }
    }
}
