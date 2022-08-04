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
        private HttpClient Http { get; set; }
        [Inject]
        private IMemberLocalStorage LocalStorage { get; set; }
        [Inject]
        private IApiRequester Requester { get; set; } = default!;
        private List<ProjectModel> _projects = new List<ProjectModel>();
        private List<MemberModel> _members = new List<MemberModel>();
        private ProjectModel _projectTarget = new ProjectModel() { IdProject = 0 };
        private bool _displayDetailsDialog;
        private bool _displayEditDialog;
        private bool _isMemberConnected;
        private bool _displayAddDialog;
        private string _token;

        protected override async Task OnInitializedAsync()
        {
            _isMemberConnected = await LocalStorage.HasToken();
            if (_isMemberConnected)
            {
                _token = await LocalStorage.GetToken();
                _projects = await Requester.Get<List<ProjectModel>>("Project", _token);
                _members = await Requester.Get<List<MemberModel>>("Member", _token);
            }
        }
        private async Task RefreshProjectList()
        {
            _isMemberConnected = await LocalStorage.HasToken();
            if (_isMemberConnected)
            {
                _token = await LocalStorage.GetToken();
                _projects = await Requester.Get<List<ProjectModel>>("Project", _token);
            }
        }
        private async Task DeleteProject(int id)
        {
            _isMemberConnected = await LocalStorage.HasToken();
            if (_isMemberConnected)
            {
                _token = await LocalStorage.GetToken();
                Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
                await Http.DeleteAsync($"https://localhost:7051/api/Project/{id}");
                await RefreshProjectList();
            }
        }

        private void DisplayDetailsDialog(ProjectModel project)
        {
            _displayDetailsDialog = !_displayDetailsDialog;
            if (_displayDetailsDialog)
            {
                _displayAddDialog = false;
                _displayEditDialog = false;
                _projectTarget = project;

            }
        }
        private void DisplayEditDialog(ProjectModel project)
        {
            _displayEditDialog = !_displayEditDialog;
            if (_displayEditDialog)
            {
                _displayAddDialog = false;
                _displayDetailsDialog = false;
                _projectTarget = project;
            }
        }
        private void DisplayAddDialog()
        {
            _displayAddDialog = !_displayAddDialog;
            if (_displayAddDialog)
            {
                _displayEditDialog = false;
                _displayDetailsDialog = false;
            }
        }
        private async Task ConfirmEdit()
        {
            await RefreshProjectList();
            _displayEditDialog = false;
        }
        private void CloseDetailsDialog()
        {
            _displayDetailsDialog = false;
        }
        private void CloseAddDialog()
        {
            _displayAddDialog = false;
        }
        private void CloseEditDialog()
        {
            _displayEditDialog = false;
        }
    }
}
