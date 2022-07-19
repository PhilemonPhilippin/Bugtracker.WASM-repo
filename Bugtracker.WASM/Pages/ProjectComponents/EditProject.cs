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
        private HttpClient Http { get; set; }
        [Inject]
        private IMemberLocalStorage LocalStorage { get; set; }
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

            _isMemberConnected = await LocalStorage.HasToken();
            if (_isMemberConnected)
            {
                _token = await LocalStorage.GetToken();
                Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
                _members = await Http.GetFromJsonAsync<List<MemberModel>>("https://localhost:7051/api/Member");
            }
        }
        private async Task SubmitEdit()
        {
            _isMemberConnected = await LocalStorage.HasToken();
            if (_isMemberConnected)
            {
                _displayNameTaken = false;
                ProjectModel projectModel = EditedProject.ToModel();

                _token = await LocalStorage.GetToken();
                Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
                using HttpResponseMessage response = await Http.PutAsJsonAsync("https://localhost:7051/api/Project", projectModel);
                if (!response.IsSuccessStatusCode)
                {
                    string message = await response.Content.ReadAsStringAsync();
                    if (message.Contains("Name already exists."))
                        _displayNameTaken = true;
                }
                else
                    await OnConfirm.InvokeAsync();
            }
        }
    }
}
