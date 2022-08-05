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
        private IMemberLocalStorage LocalStorage { get; set; } = default!;
        [Inject]
        private IApiRequester Requester { get; set; } = default!;
        [Parameter]
        public ProjectModel ProjectTarget { get; set; }
        [Parameter]
        public EventCallback OnCancel { get; set; }
        private MemberModel manager = new MemberModel();
        private bool _isMemberConnected;
        private string _token;
        protected override async Task OnInitializedAsync()
        {
            _isMemberConnected = await LocalStorage.HasToken();
            if (_isMemberConnected)
            {
                _token = await LocalStorage.GetToken();
                manager = await Requester.Get<MemberModel>($"Member/{ProjectTarget.Manager}", _token);
            }
        }
    }
}
