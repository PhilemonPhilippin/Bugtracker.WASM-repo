using Bugtracker.WASM.Models;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Bugtracker.WASM.Pages.MemberComponents
{
    public partial class DisplayMembers
    {
        [Inject]
        public HttpClient Http { get; set; }
        [Parameter]
        public string Token { get; set; }
        private List<MemberModel> _Members { get; set; } = new List<MemberModel>();

        private bool _isEditMemberDialogOpen = false;
        private int _memberEditId;
        protected override async Task OnInitializedAsync()
        {
            if (Token is not null)
                Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

            _Members = await Http.GetFromJsonAsync<List<MemberModel>>("https://localhost:7051/api/Member");
        }
        private async Task DeleteMember(int id)
        {
            await Http.DeleteAsync($"https://localhost:7051/api/Member/{id}");
            await RefreshDisplayMembers();
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
            await RefreshDisplayMembers();
        }
        private async Task RefreshDisplayMembers()
        {
            _Members = await Http.GetFromJsonAsync<List<MemberModel>>("https://localhost:7051/api/Member");
        }
    }
}
