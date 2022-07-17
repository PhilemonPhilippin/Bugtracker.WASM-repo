using Bugtracker.WASM.Models;
using Bugtracker.WASM.Models.MemberModels;
using Bugtracker.WASM.Tools;
using Microsoft.AspNetCore.Components;
using static System.Net.WebRequestMethods;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Bugtracker.WASM.Pages.ProjectComponents
{
    public partial class DetailsProject
    {
        [Inject]
        private HttpClient Http { get; set; }
        [Inject]
        private IMemberLocalStorage LocalStorage { get; set; }
        [Parameter]
        public ProjectModel ProjectTarget { get; set; }
        [Parameter]
        public EventCallback OnCancel { get; set; }
        private MemberModel manager = new MemberModel();
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
                manager = await Http.GetFromJsonAsync<MemberModel>($"https://localhost:7051/api/Member/{ProjectTarget.Manager}");
            }
        }
    }
}
