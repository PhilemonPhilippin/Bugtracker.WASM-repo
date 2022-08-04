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
        private List<MemberModel> _members = new List<MemberModel>();
        private MemberModel _memberTarget = new MemberModel() { IdMember = 0 };
        private string _token;
        private bool _displayEditDialog;
        private bool _displayDetailsDialog;
        private bool _isMemberConnected;

        protected override async Task OnInitializedAsync()
        {
            await RefreshMemberList();
        }
        private async Task RefreshMemberList()
        {
            _isMemberConnected = await LocalStorage.HasToken();
            if (_isMemberConnected)
            {
                _token = await LocalStorage.GetToken();
                Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
                _members = await Http.GetFromJsonAsync<List<MemberModel>>("https://localhost:7051/api/Member");
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
                await RefreshMemberList();
            }
        }
        private void DisplayDetailsDialog(MemberModel member)
        {
            _displayDetailsDialog = !_displayDetailsDialog;
            if (_displayDetailsDialog)
            {
                _displayEditDialog = false;
                _memberTarget = member;
            }
        }

        private void DisplayEditDialog(MemberModel member)
        {
            _displayEditDialog = !_displayEditDialog;
            if (_displayEditDialog)
            {
                _displayDetailsDialog = false;
                _memberTarget = member;
            }
        }
        private async Task ConfirmEdit()
        {
            _displayEditDialog = false;
            await RefreshMemberList();
        }
        private void CloseDetailsDialog()
        {
            _displayDetailsDialog = false;
        }
        private void CloseEditDialog()
        {
            _displayEditDialog = false;
        }
    }
}
