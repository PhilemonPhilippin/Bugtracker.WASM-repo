using Bugtracker.WASM.Models.MemberModels;
using Bugtracker.WASM.Models;
using Bugtracker.WASM.Tools;
using Microsoft.AspNetCore.Components;

namespace Bugtracker.WASM.Shared.ProjectComponents
{
    public partial class ProjectList : ComponentBase
    {
        [Inject]
        private IMemberLocalStorage LocalStorage { get; set; } = default!;
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
            _token = await LocalStorage.GetToken();
            if (_token is not null)
            {
                _isMemberConnected = true;
                _projects = await Requester.Get<List<ProjectModel>>("Project", _token);
                _members = await Requester.Get<List<MemberModel>>("Member", _token);
            }
        }
        private async Task RefreshProjectList()
        {
            _token = await LocalStorage.GetToken();
            if (_token is not null)
            {
                _isMemberConnected = true;
                _projects = await Requester.Get<List<ProjectModel>>("Project", _token);
            }
        }
        private async Task DeleteProject(int id)
        {
            _token = await LocalStorage.GetToken();
            if (_token is not null)
            {
                _isMemberConnected = true;
                await Requester.Delete($"Project/{id}", _token);
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
