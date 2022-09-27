using Bugtracker.WASM.Models.MemberModels;
using Bugtracker.WASM.Models;
using Bugtracker.WASM.Tools;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Bugtracker.WASM.Shared.ProjectComponents
{
    public partial class ProjectDetail : ComponentBase
    {
        [Inject]
        private IMemberLocalStorage LocalStorage { get; set; } = default!;
        [Inject]
        private IApiRequester Requester { get; set; } = default!;
        [Inject]
        private IJSRuntime JS { get; set; } = default!;
        [Parameter]
        public ProjectModel ProjectTarget { get; set; }
        [Parameter]
        public EventCallback OnCancel { get; set; }
        private MemberModel manager = new MemberModel();
        private ElementReference focusRef;
        private IJSObjectReference? module;
        private bool _isMemberConnected;
        private string _token;
        protected override async Task OnInitializedAsync()
        {
            _token = await LocalStorage.GetToken();
            if (_token is not null)
            {
                _isMemberConnected = true;
                if (ProjectTarget.Manager is not null)
                {
                    manager = await Requester.Get<MemberModel>($"Member/{ProjectTarget.Manager}", _token);
                }
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                module = await JS.InvokeAsync<IJSObjectReference>("import", "./js/mainscript.js");
                await module.InvokeVoidAsync("ScrollToRef", focusRef);
            }
        }
    }
}
