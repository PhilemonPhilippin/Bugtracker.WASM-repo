using Bugtracker.WASM.Models.MemberModels;
using Bugtracker.WASM.Models;
using static System.Net.WebRequestMethods;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components;
using Bugtracker.WASM.Tools;
using System.Net.Http.Json;

namespace Bugtracker.WASM.Pages.ProjectComponents
{
    public partial class DisplayMyProjects
    {
        [Inject]
        private HttpClient Http { get; set; }
        [Inject]
        private IMemberLocalStorage LocalStorage { get; set; }
        private List<ProjectModel> _projects = new List<ProjectModel>();
        private List<ProjectModel> _myProjects = new List<ProjectModel>();
        private List<MemberModel> _members = new List<MemberModel>();
        private List<AssignModel> _assigns = new List<AssignModel>();
        private ProjectModel _projectTarget = new ProjectModel() { IdProject = 0 };
        private int _myMemberId;
        private string _token;
        private bool _isMemberConnected;
        private bool _displayProjectDetailsDialog;
        private bool _displayEditProjectDialog;

        protected override async Task OnInitializedAsync()
        {
            _isMemberConnected = await LocalStorage.HasToken();
            if (_isMemberConnected)
            {
                _token = await LocalStorage.GetToken();
                Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
                _projects = await Http.GetFromJsonAsync<List<ProjectModel>>("https://localhost:7051/api/Project");
                _members = await Http.GetFromJsonAsync<List<MemberModel>>("https://localhost:7051/api/Member");
                _myMemberId = await Http.GetFromJsonAsync<int>("https://localhost:7051/api/Member/idfromjwt");
                _assigns = await Http.GetFromJsonAsync<List<AssignModel>>("https://localhost:7051/api/Assign");
                // Je crée une liste avec les projects id pour lesquels le membre connecté est assigné.
                List<int> myProjectsId = new List<int>();
                foreach (AssignModel assign in _assigns)
                {
                    if (assign.Member == _myMemberId)
                    {
                        myProjectsId.Add(assign.Project);
                    }
                }
                // Je fais une liste avec mes projets pour lesquels l'id se trouve dans la liste que j'ai créée plus haut.
                if (myProjectsId.Count > 0)
                    _myProjects = _projects.Where(prj => myProjectsId.Contains(prj.IdProject)).ToList();
            }
        }
        private async Task RefreshProjectsList()
        {
            _isMemberConnected = await LocalStorage.HasToken();
            if (_isMemberConnected)
            {
                _token = await LocalStorage.GetToken();
                Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
                _projects = await Http.GetFromJsonAsync<List<ProjectModel>>("https://localhost:7051/api/Project");
                _assigns = await Http.GetFromJsonAsync<List<AssignModel>>("https://localhost:7051/api/Assign");
                // Je crée une liste avec les projects id pour lesquels le membre connecté est assigné.
                List<int> myProjectsId = new List<int>();
                foreach (AssignModel assign in _assigns)
                {
                    if (assign.Member == _myMemberId)
                    {
                        myProjectsId.Add(assign.Project);
                    }
                }
                // Je fais une liste avec mes projets pour lesquels l'id se trouve dans la liste que j'ai créée plus haut.
                if (myProjectsId.Count > 0)
                    _myProjects = _projects.Where(prj => myProjectsId.Contains(prj.IdProject)).ToList();
            }
        }
        private async Task ConfirmProjectEdit()
        {
            await RefreshProjectsList();
            _displayEditProjectDialog = false;
        }
        private void DisplayProjectDetailsDialog(ProjectModel project)
        {
            _displayProjectDetailsDialog = !_displayProjectDetailsDialog;
            if (_displayProjectDetailsDialog)
            {
                _displayEditProjectDialog = false;
                _projectTarget = project;
            }
        }
        private void DisplayProjectEditDialog(ProjectModel project)
        {
            _displayEditProjectDialog = !_displayEditProjectDialog;
            if (_displayEditProjectDialog)
            {
                _displayProjectDetailsDialog = false;
                _projectTarget = project;
            }
        }
        private void CloseDetailsDialog()
        {
            _displayProjectDetailsDialog = false;
        }
        private void CloseEditDialog()
        {
            _displayEditProjectDialog = false;
        }
    }
}
