using Bugtracker.WASM.Tools;
using Microsoft.AspNetCore.Components;
using static System.Net.WebRequestMethods;
using System.Net.Http.Headers;
using Bugtracker.WASM.Models;
using System.Net.Http.Json;
using System.Diagnostics.Metrics;
using Bugtracker.WASM.Models.MemberModels;

namespace Bugtracker.WASM.Pages.ProjectComponents
{
    public partial class DisplayProjects
    {
        [Inject]
        public HttpClient Http { get; set; }
        [Inject]
        IMemberLocalStorage LocalStorage { get; set; }
        private List<ProjectModel> _projects = new List<ProjectModel>();
        private List<MemberModel> _members = new List<MemberModel>();
        private ProjectModel _projectTarget;
        private bool _displayProjectDetailsDialog;
        private bool _displayEditProjectDialog;
        private bool _isMemberConnected;
        private bool _displayAddProjectDialog;
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
                _members = await Http.GetFromJsonAsync<List<MemberModel>>("https://localhost:7051/api/Member");
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
                _displayAddProjectDialog = false;
                _displayEditProjectDialog = false;
                _displayProjectDetailsDialog = true;
                _projectTarget = project;
            }
        }
        private void DisplayProjectEditDialog(ProjectModel project)
        {
            if (_displayEditProjectDialog)
                _displayEditProjectDialog = false;
            else
            {
                _displayAddProjectDialog = false;
                _displayProjectDetailsDialog = false;
                _displayEditProjectDialog = true;
                _projectTarget = project;
            }
        }
        private void CloseDetailsDialog()
        {
            _displayProjectDetailsDialog = false;
        }
        private void CloseAddDialog()
        {
            _displayAddProjectDialog = false;
        }
        private void CloseEditDialog()
        {
            _displayEditProjectDialog = false;
        }

        private void DisplayAddProjectDialog()
        {
            if (_displayAddProjectDialog)
                _displayAddProjectDialog = false;
            else
            {
                _displayEditProjectDialog = false;
                _displayProjectDetailsDialog = false;
                _displayAddProjectDialog = true;
            }
        }
       private async Task ConfirmProjectEdit()
        {
            await RefreshProjectsList();
            _displayEditProjectDialog = false;
        }
    }
}
