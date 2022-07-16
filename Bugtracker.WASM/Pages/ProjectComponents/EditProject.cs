using Bugtracker.WASM.Models;
using Microsoft.AspNetCore.Components;
using static System.Net.WebRequestMethods;
using System.Net.Http.Headers;
using Bugtracker.WASM.Tools;
using Bugtracker.WASM.Mappers;
using System.Net.Http.Json;

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
        private bool _isMemberConnected;
        private bool _displayNameTaken;
        private string _token;


        protected override async Task OnInitializedAsync()
        {
            EditedProject.IdProject = ProjectTarget.IdProject;
            EditedProject.Name = ProjectTarget.Name;
            EditedProject.Description = ProjectTarget.Description;
            EditedProject.Manager = ProjectTarget.Manager;
        }

        private async Task SubmitEdit()
        {
            _token = await LocalStorage.GetToken();
            if (_token is null)
                _isMemberConnected = false;
            else
                _isMemberConnected = true;

            if (_isMemberConnected)
            {
                _displayNameTaken = false;

                ProjectModel projectModel = EditedProject.ToModel();

                Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
                HttpResponseMessage response = await Http.PutAsJsonAsync("https://localhost:7051/api/Project", projectModel);
                if (!response.IsSuccessStatusCode)
                {
                    string message = await response.Content.ReadAsStringAsync();
                    if (message.Contains("Name already exists."))
                        _displayNameTaken = true;
                }
                else
                {
                    await OnConfirm.InvokeAsync();
                }
            }
        }
    }
}
