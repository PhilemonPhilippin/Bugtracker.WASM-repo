using Bugtracker.WASM.Models;
using Microsoft.AspNetCore.Components;

namespace Bugtracker.WASM.Pages.MemberComponents
{
    public partial class DetailsMember
    {
        [Parameter]
        public MemberModel MemberTarget { get; set; }
        [Parameter]
        public EventCallback OnCancel { get; set; }
    }
}
