using Bugtracker.WASM.Models.MemberModels;
using Bugtracker.WASM.Tools;
using Microsoft.AspNetCore.Components;

namespace Bugtracker.WASM.Shared.MemberComponents
{
    public partial class MemberProfile
    {
        [Inject]
        private IMemberLocalStorage LocalStorage { get; set; } = default!;
        [Inject]
        private IApiRequester Requester { get; set; } = default!;
        private MemberModel _member = new MemberModel();
        private bool _isMemberConnected;
        private bool _displayEditProfileDialog;
        private bool _displayChangePswdDialog;
        private string _token;
        private int _memberId;

        protected override async Task OnInitializedAsync()
        {
            _token = await LocalStorage.GetToken();
            if (_token is not null)
            {
                _isMemberConnected = true;
                _memberId = await Requester.Get<int>("Member/idfromjwt", _token);
                _member = await Requester.Get<MemberModel>($"Member/{_memberId}", _token);
            }
        }
        private async Task RefreshMember()
        {
            _token = await LocalStorage.GetToken();
            if (_token is not null)
            {
                _isMemberConnected = true;
                _member = await Requester.Get<MemberModel>($"Member/{_memberId}", _token);
            }
        }
        private async Task ConfirmEdit()
        {
            _displayEditProfileDialog = false;
            await RefreshMember();
        }
        private void DisplayPswdDialog()
        {
            _displayChangePswdDialog = !_displayChangePswdDialog;
            if (_displayChangePswdDialog)
                _displayEditProfileDialog = false;
        }
        private void DisplayEditProfileDialog()
        {
            _displayEditProfileDialog = !_displayEditProfileDialog;
            if (_displayEditProfileDialog)
                _displayChangePswdDialog = false;
        }
        private void ClosePswdDialog()
        {
            _displayChangePswdDialog = false;
        }
        private void CloseEditDialog()
        {
            _displayEditProfileDialog = false;
        }
    }
}
