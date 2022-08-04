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
                Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
                _memberId = await Http.GetFromJsonAsync<int>("https://localhost:7051/api/Member/idfromjwt");
                _member = await Http.GetFromJsonAsync<MemberModel>($"https://localhost:7051/api/Member/{_memberId}");
            }
        }
        private async Task RefreshMember()
        {
            _isMemberConnected = await LocalStorage.HasToken();
            if (_isMemberConnected)
            {
                _token = await LocalStorage.GetToken();
                Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
                _member = await Http.GetFromJsonAsync<MemberModel>($"https://localhost:7051/api/Member/{_memberId}");
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
