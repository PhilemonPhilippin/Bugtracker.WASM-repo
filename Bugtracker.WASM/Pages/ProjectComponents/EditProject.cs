using Bugtracker.WASM.Models;
using Microsoft.AspNetCore.Components;
using static System.Net.WebRequestMethods;
using System.Net.Http.Headers;
using Bugtracker.WASM.Tools;
using Bugtracker.WASM.Mappers;
using System.Net.Http.Json;
using Bugtracker.WASM.Models.MemberModels;

namespace Bugtracker.WASM.Pages.ProjectComponents
{
    public partial class EditProject
    {
        [Inject]
        private IMemberLocalStorage LocalStorage { get; set; } = default!;
        [Inject]
        private IApiRequester Requester { get; set; } = default!;
        [Parameter]
        public EventCallback OnCancel { get; set; }
        [Parameter]
        public EventCallback OnConfirm { get; set; }
        [Parameter]
        public ProjectModel ProjectTarget { get; set; }
        private ProjectFormModel EditedProject { get; set; } = new ProjectFormModel();
        private List<MemberModel> _members = new List<MemberModel>();
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
