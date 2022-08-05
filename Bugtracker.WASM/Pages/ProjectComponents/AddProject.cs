using Bugtracker.WASM.Mappers;
using Bugtracker.WASM.Models;
using Bugtracker.WASM.Models.MemberModels;
using Bugtracker.WASM.Tools;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Headers;
using System.Net.Http.Json;

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
            _isMemberConnected = await LocalStorage.HasToken();
            if (_isMemberConnected)
            {
                _token = await LocalStorage.GetToken();
                _members = await Requester.Get<List<MemberModel>>("Member", _token);
            }
        }
        private async Task SubmitAdd()
        {
            _isMemberConnected = await LocalStorage.HasToken();
            if (_isMemberConnected)
            {
                _token = await LocalStorage.GetToken();
                _displayNameTaken = false;
                _isProjectAdded = false;
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
