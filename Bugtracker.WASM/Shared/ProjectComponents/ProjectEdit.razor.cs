using Bugtracker.WASM.Models.MemberModels;
using Bugtracker.WASM.Models;
using Bugtracker.WASM.Tools;
using Microsoft.AspNetCore.Components;
using Bugtracker.WASM.Mappers;
using Microsoft.JSInterop;

namespace Bugtracker.WASM.Shared.ProjectComponents
{
    public partial class ProjectEdit : ComponentBase
    {
        [Inject]
        private IMemberLocalStorage LocalStorage { get; set; } = default!;
        [Inject]
        private IApiRequester Requester { get; set; } = default!;
        [Inject]
        private IJSRuntime JS { get; set; } = default!;
        [Parameter]
        public EventCallback OnCancel { get; set; }
        [Parameter]
        public EventCallback OnConfirm { get; set; }
        [Parameter]
        public ProjectModel ProjectTarget { get; set; }
        private ProjectFormModel EditedProject { get; set; } = new ProjectFormModel();
        private List<MemberModel> _members = new List<MemberModel>();
        private ElementReference focusRef;
        private IJSObjectReference? module;
        private bool _isMemberConnected;
        private bool _displayNameTaken;
        private string _token;
        protected override async Task OnInitializedAsync()
        {
            EditedProject.IdProject = ProjectTarget.IdProject;
            EditedProject.Name = ProjectTarget.Name;
            EditedProject.Description = ProjectTarget.Description;
            EditedProject.Manager = ProjectTarget.Manager;

            _token = await LocalStorage.GetToken();
            if (_token is not null)
            {
                _isMemberConnected = true;
                _members = await Requester.Get<List<MemberModel>>("Member", _token);
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

        private async Task SubmitEdit()
        {
            _token = await LocalStorage.GetToken();
            if (_token is not null)
            {
                _isMemberConnected = true;
                _displayNameTaken = false;
                ProjectModel projectModel = EditedProject.ToModel();

                using HttpResponseMessage response = await Requester.Put(projectModel, "Project", _token);
                if (!response.IsSuccessStatusCode)
                {
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    if (errorMessage.Contains("Name already exists."))
                        _displayNameTaken = true;
                }
                else
                    await OnConfirm.InvokeAsync();
            }
        }
    }
}
