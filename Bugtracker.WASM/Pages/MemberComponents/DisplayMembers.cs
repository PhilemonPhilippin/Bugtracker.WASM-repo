using Bugtracker.WASM.Models.MemberModels;
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
        private List<MemberNoPswdModel> _members = new List<MemberNoPswdModel>();
        private MemberNoPswdModel _memberTarget = new MemberNoPswdModel() { IdMember = 0 };
        private string _token;
        private bool _displayMemberEditDialog;
        private bool _displayMemberDetailsDialog;
        private bool _isMemberConnected;
        protected override async Task OnInitializedAsync()
        {
            _isMemberConnected = await LocalStorage.HasToken();
            if (_isMemberConnected)
            {
                _token = await LocalStorage.GetToken();
                Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
                _members = await Http.GetFromJsonAsync<List<MemberNoPswdModel>>("https://localhost:7051/api/Member");
            }
        }
        private async Task DeleteMember(int id)
        {
            _isMemberConnected = await LocalStorage.HasToken();
            if (_isMemberConnected)
            {
                _token = await LocalStorage.GetToken();
                Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
                await Http.DeleteAsync($"https://localhost:7051/api/Member/{id}");
                await RefreshMembersList();
            }
        }
        private void DisplayMemberDetailsDialog(MemberNoPswdModel member)
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
        private void DisplayMemberEditDialog(MemberNoPswdModel member)
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
            _isMemberConnected = await LocalStorage.HasToken();
            if (_isMemberConnected)
            {
                _token = await LocalStorage.GetToken();
                Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
                _members = await Http.GetFromJsonAsync<List<MemberNoPswdModel>>("https://localhost:7051/api/Member");

            }
        }
    }
}
