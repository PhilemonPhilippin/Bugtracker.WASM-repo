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
        private HttpClient Http { get; set; }
        [Inject]
        private IMemberLocalStorage LocalStorage { get; set; }
        [Parameter]
        public EventCallback OnCancel { get; set; }
        [Parameter]
        public EventCallback OnConfirm { get; set; }
        private List<MemberNoPswdModel> _members = new List<MemberNoPswdModel>();
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
                Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
                _members = await Http.GetFromJsonAsync<List<MemberNoPswdModel>>("https://localhost:7051/api/Member");
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

                Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
                using HttpResponseMessage response = await Http.PostAsJsonAsync("https://localhost:7051/api/Project", projectModel);

                if (!response.IsSuccessStatusCode)
                {
                    string message = await response.Content.ReadAsStringAsync();
                    if (message.Contains("Name already exists."))
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
