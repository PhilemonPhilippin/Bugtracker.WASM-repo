using Bugtracker.WASM.Models.MemberModels;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Bugtracker.WASM.Shared.MemberComponents
{
    public partial class MemberDetail : ComponentBase
    {
        [Inject]
        IJSRuntime JS { get; set; } = default!;

        [Parameter]
        public MemberModel MemberTarget { get; set; }
        [Parameter]
        public EventCallback OnCancel { get; set; }
        private ElementReference focusRef;
        private IJSObjectReference? module;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                module = await JS.InvokeAsync<IJSObjectReference>("import", "./js/mainscript.js");
                await module.InvokeVoidAsync("ScrollToRef", focusRef);
            }
        }

        private string RoleName(int role)
        {
            switch (role)
            {
                case 2:
                    return "Tester";
                case 3:
                    return "Project Manager";
                case 4:
                    return "Administrator";
                case 5:
                    return "Web Master";
                default:
                    return "Developer";
            }
        }
    }
}
