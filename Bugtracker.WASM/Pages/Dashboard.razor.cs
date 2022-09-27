using Bugtracker.WASM.Models.MemberModels;
using Bugtracker.WASM.Tools;
using Microsoft.AspNetCore.Components;

namespace Bugtracker.WASM.Pages
{
    public partial class Dashboard : ComponentBase
    {
        [Inject]
        private IMemberLocalStorage LocalStorage { get; set; } = default!;
        [Inject]
        private NavigationManager NavManager { get; set; } = default!;
        [Inject]
        private IApiRequester Requester { get; set; } = default!;

        private bool _isMemberConnected;
        private bool _displayMyTicketsComponent = true;
        private bool _displayMyProjectsComponent;
        private string _token;
        private int _myMemberId;
        private MemberModel _myMemberModel = new();
        protected override async Task OnInitializedAsync()
        {
            _token = await LocalStorage.GetToken();
            if (_token is null)
            {
                _isMemberConnected = false;
                NavManager.NavigateTo("/account");
            }
            else
            {
                _isMemberConnected = true;
                _myMemberId = await Requester.Get<int>("Member/idfromjwt", _token);
                _myMemberModel = await Requester.Get<MemberModel>($"Member/{_myMemberId}", _token);
            }
        }
        private void ToAccount()
        {
            NavManager.NavigateTo("/account");
        }

        private void DisplayMyTicketsComponent()
        {
            _displayMyTicketsComponent = !_displayMyTicketsComponent;
            if (_displayMyTicketsComponent)
                _displayMyProjectsComponent = false;
        }
        private void DisplayMyProjectsComponent()
        {
            _displayMyProjectsComponent = !_displayMyProjectsComponent;
            if (_displayMyProjectsComponent)
                _displayMyTicketsComponent = false;
        }
    }
}
