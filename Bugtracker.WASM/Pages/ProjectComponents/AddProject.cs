using Bugtracker.WASM.Mappers;
using Bugtracker.WASM.Models;
using Bugtracker.WASM.Models.MemberModels;
using Bugtracker.WASM.Tools;
using Microsoft.AspNetCore.Components;

namespace Bugtracker.WASM.Pages.ProjectComponents
{
    public partial class AddProject
    {
        [Inject]
        private IMemberLocalStorage LocalStorage { get; set; } = default!;
        [Inject]
        private IApiRequester Requester { get; set; } = default!;
        [Parameter]
        public EventCallback OnCancel { get; set; }
        [Parameter]
        public EventCallback OnConfirm { get; set; }
        private List<MemberModel> _members = new List<MemberModel>();
        private ProjectFormModel AddedProject { get; set; } = new ProjectFormModel() { IdProject = default };

        private bool _displayNameTaken;
        private bool _isMemberConnected;
        private bool _isProjectAdded;
        private string _token;

        protected override async Task OnInitializedAsync()
        {
            _token = await LocalStorage.GetToken();
            if (_token is not null)
            {
                _isMemberConnected = true;
                _members = await Requester.Get<List<MemberModel>>("Member", _token);
            }
        }
        private async Task SubmitAdd()
        {
            _token = await LocalStorage.GetToken();
            if (_token is not null)
            {
                _isMemberConnected = true;
                _displayNameTaken = false;
                _isProjectAdded = false;

                AddedProject.Disabled = false;
                ProjectModel projectModel = AddedProject.ToModel();

                using HttpResponseMessage response = await Requester.Post(projectModel, "Project", _token);

                if (!response.IsSuccessStatusCode)
                {
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    if (errorMessage.Contains("Name already exists."))
                        _displayNameTaken = true;
                }
                else
                {
                    _isProjectAdded = true;
                    await OnConfirm.InvokeAsync();
                    AddedProject = new ProjectFormModel() { IdProject = default };
                }
            }
        }
    }
}
