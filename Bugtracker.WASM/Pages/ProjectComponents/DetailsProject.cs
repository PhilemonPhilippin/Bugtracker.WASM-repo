using Bugtracker.WASM.Models;
using Microsoft.AspNetCore.Components;

namespace Bugtracker.WASM.Pages.ProjectComponents
{
    public partial class DetailsProject
    {
        [Parameter]
        public ProjectModel ProjectTarget { get; set; }
        [Parameter]
        public EventCallback OnCancel { get; set; }
    }
}
