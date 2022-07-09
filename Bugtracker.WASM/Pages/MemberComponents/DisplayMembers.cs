using Bugtracker.WASM.Models;
using Bugtracker.WASM.Tools;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Bugtracker.WASM.Pages.MemberComponents
{
    public partial class DisplayMembers
    {
        [Inject]
        public HttpClient Http { get; set; }
        [Inject]
        private IMemberLocalStorage LocalStorage { get; set; }
        private List<MemberModel> _members = new List<MemberModel>();
        private MemberModel _memberTarget;
        private string _token;
        private bool _displayMemberEditDialog;
        private bool _displayMemberDetailsDialog;
        private bool _isMemberConnected;
        private int _memberTargetId;
        protected override async Task OnInitializedAsync()
        {
            //await AskTokenValidation();
            _token = await LocalStorage.GetToken();
            if (_token is null)
                _isMemberConnected = false;
            else
                _isMemberConnected = true;

            if (_isMemberConnected)
            {
                Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
                _members = await Http.GetFromJsonAsync<List<MemberModel>>("https://localhost:7051/api/Member");
            }
        }
        private async Task DeleteMember(int id)
        {
            await Http.DeleteAsync($"https://localhost:7051/api/Member/{id}");
            await RefreshMembersList();
        }

        private void DisplayMemberDetailsDialog(MemberModel member)
        {
            if (_displayMemberDetailsDialog)
                _displayMemberDetailsDialog = false;
            else
            {
                _displayMemberEditDialog = false;
                _displayMemberDetailsDialog = true;
                _memberTarget = member;
            }
        }
        private void CloseDetailsDialog()
        {
            _displayMemberDetailsDialog = false;
        }
        private void DisplayMemberEditDialog(MemberModel member)
        {
            if (_displayMemberEditDialog)
                _displayMemberEditDialog = false;
            else
            {
                _displayMemberDetailsDialog = false;
                _displayMemberEditDialog = true;
                _memberTarget = member;

            }
        }
        private void CloseMemberEditDialog()
        {
            _displayMemberEditDialog = false;
        }
        private async Task ConfirmMemberEdit()
        {
            _displayMemberEditDialog = false;
            await RefreshMembersList();
        }
        private async Task RefreshMembersList()
        {
            //await AskTokenValidation();
            _token = await LocalStorage.GetToken();
            if (_token is null)
                _isMemberConnected = false;
            else
                _isMemberConnected = true;

            if (_isMemberConnected)
            {
                Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
                _members = await Http.GetFromJsonAsync<List<MemberModel>>("https://localhost:7051/api/Member");
            }
        }
        //private async Task AskTokenValidation()
        //{
        //    _token = await LocalStorage.GetToken();
        //    if (_token is null)
        //        _isMemberConnected = false;
        //    else
        //    {
        //        Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
        //        HttpResponseMessage response = await Http.GetAsync("https://localhost:7051/api/Member/token");
        //        if (!response.IsSuccessStatusCode)
        //            _isMemberConnected = false;
        //        else
        //        {
        //            ConnectedMemberModel connectedMember = await response.Content.ReadFromJsonAsync<ConnectedMemberModel>();
        //            await LocalStorage.SetToken(connectedMember.Token);
        //            _isMemberConnected = true;
        //        }
        //    }
        //}
    }
}
