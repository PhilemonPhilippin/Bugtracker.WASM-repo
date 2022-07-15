using Bugtracker.WASM.Tools;
using Microsoft.AspNetCore.Components;
using static System.Net.WebRequestMethods;
using System.Net.Http.Headers;
using Bugtracker.WASM.Models;
using System.Net.Http.Json;
using System.Diagnostics.Metrics;

namespace Bugtracker.WASM.Pages.ProjectComponents
{
    public partial class DisplayProjects
    {
        [Inject]
        public HttpClient Http { get; set; }
        [Inject]
        IMemberLocalStorage LocalStorage { get; set; }
        private List<ProjectModel> _projects = new List<ProjectModel>();
        private ProjectModel _projectTarget;
        private bool _displayProjectDetailsDialog;
        private bool _displayProjectEditDialog;
        private bool _isMemberConnected;
        private string _token;

        protected override async Task OnInitializedAsync()
        {
            _token = await LocalStorage.GetToken();
            if (_token is null)
                _isMemberConnected = false;
            else
                _isMemberConnected = true;

            if (_isMemberConnected)
            {
                Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
                _projects = await Http.GetFromJsonAsync<List<ProjectModel>>("https://localhost:7051/api/Project");
            }
        }
        private async Task DeleteProject(int id)
        {
            await Http.DeleteAsync($"https://localhost:7051/api/Project/{id}");
            await RefreshProjectsList();
        }
        private async Task RefreshProjectsList()
        {
            _token = await LocalStorage.GetToken();
            if (_token is null)
                _isMemberConnected = false;
            else
                _isMemberConnected = true;

            if (_isMemberConnected)
            {
                Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
                _projects = await Http.GetFromJsonAsync<List<ProjectModel>>("https://localhost:7051/api/Project");
            }
        }
        private void DisplayProjectDetailsDialog(ProjectModel project)
        {
            if (_displayProjectDetailsDialog)
                _displayProjectDetailsDialog = false;
            else
            {
                _displayProjectEditDialog = false;
                _displayProjectDetailsDialog = true;
                _projectTarget = project;
            }
        }
        private void DisplayProjectEditDialog(ProjectModel project)
        {
            if (_displayProjectEditDialog)
                _displayProjectEditDialog = false;
            else
            {
                _displayProjectDetailsDialog = false;
                _displayProjectEditDialog = true;
                _projectTarget = project;
            }
        }
        private void CloseDetailsDialog()
        {
            _displayProjectDetailsDialog = false;
        }
    }
}
