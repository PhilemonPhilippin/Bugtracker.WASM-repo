using Bugtracker.WASM.Tools;
using Microsoft.AspNetCore.Components;
using static System.Net.WebRequestMethods;
using System.Net.Http.Headers;
using Bugtracker.WASM.Models;
using System.Net.Http.Json;

namespace Bugtracker.WASM.Pages.ProjectComponents
{
    public partial class DisplayProjects
    {
        [Inject]
        public HttpClient Http { get; set; }
        [Inject]
        IMemberLocalStorage LocalStorage { get; set; }
        private List<ProjectModel> _projects = new List<ProjectModel>();
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
    }
}
