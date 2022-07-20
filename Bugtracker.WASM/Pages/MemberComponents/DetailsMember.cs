using Bugtracker.WASM.Models.MemberModels;
using Microsoft.AspNetCore.Components;

namespace Bugtracker.WASM.Pages.MemberComponents
{
    public partial class DetailsMember
    {
        [Parameter]
        public MemberNoPswdModel MemberTarget { get; set; }
        [Parameter]
        public EventCallback OnCancel { get; set; }
    }
}
