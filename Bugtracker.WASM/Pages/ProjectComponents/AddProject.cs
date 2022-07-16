using Bugtracker.WASM.Mappers;
using Bugtracker.WASM.Models;
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
        private ProjectAddModel AddedProject { get; set; } = new ProjectAddModel();
        private bool _isAddValid;
        private bool _displayNameTaken;
        private bool _isMemberConnected;
        private string _token;
        private async Task SubmitAdd()
        {
            _token = await LocalStorage.GetToken();
            if (_token is null)
                _isMemberConnected = false;
            else
                _isMemberConnected = true;

            if (_isMemberConnected)
            {
                _isAddValid = true;
                _displayNameTaken = false;

                ProjectModel projectModel = AddedProject.ToModel();

                Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
                HttpResponseMessage response = await Http.PostAsJsonAsync("https://localhost:7051/api/Project", projectModel);

                if (!response.IsSuccessStatusCode)
                {
                    string message = await response.Content.ReadAsStringAsync();
                    if (message.Contains("Name already exists."))
                        _displayNameTaken = true;
                }
                else
                {
                    _isAddValid = true;
                    await OnConfirm.InvokeAsync();
                    AddedProject = new ProjectAddModel();
                }
            }
        }
    }
}
