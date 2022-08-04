using Bugtracker.WASM.Models.MemberModels;
using Bugtracker.WASM.Tools;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Bugtracker.WASM.Pages.MemberComponents
{
    public partial class ProfileMember
    {
        [Inject]
        private HttpClient Http { get; set; }
        [Inject]
        private IMemberLocalStorage LocalStorage { get; set; }
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
            _isMemberConnected = await LocalStorage.HasToken();
            if (_isMemberConnected)
            {
                _token = await LocalStorage.GetToken();
                _memberId = await Requester.Get<int>("Member/idfromjwt", _token);
                _member = await Requester.Get<MemberModel>($"Member/{_memberId}", _token);
            }
        }
        private async Task RefreshMember()
        {
            _isMemberConnected = await LocalStorage.HasToken();
            if (_isMemberConnected)
            {
                _token = await LocalStorage.GetToken();
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
