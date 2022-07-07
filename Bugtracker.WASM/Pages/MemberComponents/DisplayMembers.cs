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
        private string _token;
        private bool _isEditMemberDialogOpen;
        private int _memberEditId;
        protected override async Task OnInitializedAsync()
        {
            _token = await LocalStorage.GetToken();
            if (_token is not null)
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
        private void DisplayEditMemberDialog(int id)
        {
            _isEditMemberDialogOpen = true;
            _memberEditId = id;
        }
        private void CloseEditMemberDialog()
        {
            _isEditMemberDialogOpen = false;
        }
        private async Task ConfirmMemberEdit()
        {
            _isEditMemberDialogOpen = false;
            await RefreshMembersList();
        }
        private async Task RefreshMembersList()
        {
            _token = await LocalStorage.GetToken();
            if (_token is not null)
            {
                Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
                _members = await Http.GetFromJsonAsync<List<MemberModel>>("https://localhost:7051/api/Member");
            }
        }
    }
}
