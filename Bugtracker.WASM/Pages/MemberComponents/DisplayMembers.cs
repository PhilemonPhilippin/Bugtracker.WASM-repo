using Bugtracker.WASM.Models;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace Bugtracker.WASM.Pages.MemberComponents
{
    public partial class DisplayMembers
    {
        [Inject]
        public HttpClient Http { get; set; }
        public List<MemberModel> members { get; set; } = new List<MemberModel>();
        [Parameter]
        public EventCallback OnListRefresh { get; set; }

        public bool isEditMemberDisplayed = false;
        public int idEditMemberDisplayed;
        protected override async Task OnInitializedAsync()
        {
            members = await Http.GetFromJsonAsync<List<MemberModel>>("https://localhost:7051/api/Member");
        }
        private async Task DeleteMember(int id)
        {
            await Http.DeleteAsync($"https://localhost:7051/api/Member/{id}");
            await RefreshDisplayMembers();
            await OnListRefresh.InvokeAsync();
        }
        private void DisplayEditMemberDialog(int id)
        {
            isEditMemberDisplayed = true;
            idEditMemberDisplayed = id;
        }
        private void CloseEditMemberDialog()
        {
            isEditMemberDisplayed = false;
        }
        private async Task ConfirmEditMember()
        {
            isEditMemberDisplayed = false;
            await RefreshDisplayMembers();
            await OnListRefresh.InvokeAsync();
        }
        private async Task RefreshDisplayMembers()
        {
            members = await Http.GetFromJsonAsync<List<MemberModel>>("https://localhost:7051/api/Member");
        }
    }
}
