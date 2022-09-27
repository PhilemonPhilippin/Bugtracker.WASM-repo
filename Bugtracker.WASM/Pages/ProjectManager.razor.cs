using Bugtracker.WASM.Models.MemberModels;
using Bugtracker.WASM.Tools;
using Microsoft.AspNetCore.Components;

namespace Bugtracker.WASM.Pages
{
    public partial class ProjectManager : ComponentBase
    {
        [Inject]
        private NavigationManager NavManager { get; set; } = default!;
        [Inject]
        private IMemberLocalStorage LocalStorage { get; set; } = default!;
        [Inject]
        private IApiRequester Requester { get; set; } = default!;
        private MemberModel _myMemberModel = new();
        private bool _isMemberConnected;
        private bool _displayProjectsComponent = true;
        private bool _displayTicketsComponent;
        private string _token;
        private int _myMemberId;

        protected override async Task OnInitializedAsync()
        {
            _token = await LocalStorage.GetToken();
            if (_token is not null)
            {
                _isMemberConnected = true;
                _myMemberId = await Requester.Get<int>("Member/idfromjwt", _token);
                _myMemberModel = await Requester.Get<MemberModel>($"Member/{_myMemberId}", _token);
            }
            else
            {
                _isMemberConnected = false;
                NavManager.NavigateTo("/account");
            }
        }
        private void ToAccount()
        {
            NavManager.NavigateTo("/account");
        }
        private void ToDashboard()
        {
            NavManager.NavigateTo("");
        }
        private void DisplayProjectsComponent()
        {
            _displayProjectsComponent = !_displayProjectsComponent;
            if (_displayProjectsComponent)
                _displayTicketsComponent = false;
        }
        private void DisplayTicketsComponent()
        {
            _displayTicketsComponent = !_displayTicketsComponent;
            if (_displayTicketsComponent)
                _displayProjectsComponent = false;
        }
    }
}
