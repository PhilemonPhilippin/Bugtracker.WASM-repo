using Bugtracker.WASM.Models;

namespace Bugtracker.WASM.Mappers
{
    internal static class ProjectMapper
    {
        public static ProjectModel ToModel(this ProjectFormModel formModel)
        {
            return new ProjectModel()
            {
                IdProject = (formModel.IdProject == 0) ? 0 : formModel.IdProject,
                Name = formModel.Name,
                Description = formModel.Description,
                Manager = formModel.Manager,
                Disabled = formModel.Disabled
            };
        }
    }
}
