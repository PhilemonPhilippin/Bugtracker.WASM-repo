using Bugtracker.WASM.Models.MemberModels;
using Bugtracker.WASM.Tools;
using Microsoft.AspNetCore.Components;

namespace Bugtracker.WASM.Pages
{
    public partial class Members
    {
        [Inject]
        private NavigationManager NavManager { get; set; } = default!;
        [Inject]
        private IMemberLocalStorage LocalStorage { get; set; } = default!;
        [Inject]
        private IApiRequester Requester { get; set; } = default!;

        private MemberModel _myMemberModel = new();
        private bool _isMemberConnected;
        private string _token;
        private int _myMemberId;

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
    }
}
