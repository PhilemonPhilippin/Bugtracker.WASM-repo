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
        private MemberNoPswdModel _member = new MemberNoPswdModel();
        private bool _isMemberConnected;
        private bool _displayEditProfileDialog;
        private bool _displayChangePswdDialog;
        private string _token;
        private int? _memberId;
        protected override async Task OnInitializedAsync()
        {
            _isMemberConnected = await LocalStorage.HasToken();
            if (_isMemberConnected)
            {
                _token = await LocalStorage.GetToken();
                Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
                _memberId = await Http.GetFromJsonAsync<int?>("https://localhost:7051/api/Member/idfromjwt");
                _member = await Http.GetFromJsonAsync<MemberNoPswdModel>($"https://localhost:7051/api/Member/{_memberId}");
            }
        }
        private void DisplayEditProfileDialog()
        {
            if (_displayEditProfileDialog)
                _displayEditProfileDialog = false;
            else
            {
                _displayEditProfileDialog = true;
            }
        }
        private void CloseMemberEditDialog()
        {
            _displayEditProfileDialog = false;
        }
        private async Task ConfirmMemberEdit()
        {
            _displayEditProfileDialog = false;
            await RefreshMember();
        }
        private async Task RefreshMember()
        {
            _isMemberConnected = await LocalStorage.HasToken();
            if (_isMemberConnected)
            {
                _token = await LocalStorage.GetToken();
                Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
                _member = await Http.GetFromJsonAsync<MemberNoPswdModel>($"https://localhost:7051/api/Member/{_memberId}");
            }
        }
        private void DisplayChangePswdDialog()
        {
            if (_displayChangePswdDialog)
                _displayChangePswdDialog = false;
            else
            {
                _displayChangePswdDialog = true;
            }
        }
    }
}
