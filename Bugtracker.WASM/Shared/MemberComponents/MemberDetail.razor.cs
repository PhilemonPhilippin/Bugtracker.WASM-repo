using Bugtracker.WASM.Models.MemberModels;
using Microsoft.AspNetCore.Components;

namespace Bugtracker.WASM.Shared.MemberComponents
{
    public partial class MemberDetail
    {
        [Parameter]
        public MemberModel MemberTarget { get; set; }
        [Parameter]
        public EventCallback OnCancel { get; set; }
    }
}
