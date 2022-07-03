using Bugtracker.WASM.Models;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace Bugtracker.WASM.Pages.MemberComponents
{
    public partial class EditMember
    {
        [Inject]
        public HttpClient Http { get; set; }
        [Parameter]
        public int TargetId { get; set; }
        [Parameter]
        public EventCallback OnClose { get; set; }
        [Parameter]
        public EventCallback OnConfirm { get; set; }
        public MemberEditModel MemberEditModel { get; set; } = new MemberEditModel();
        public bool isPseudoTaken = false;
        public bool isEmailTaken = false;
        public bool hasMemberBeenEdited = false;

        protected override async Task OnInitializedAsync()
        {
            HttpResponseMessage response = await Http.GetAsync($"https://localhost:7051/api/Member/{TargetId}");
            if (response.IsSuccessStatusCode)
            {
                MemberModel memberModel = await response.Content.ReadFromJsonAsync<MemberModel>();
                MemberEditModel.IdMember = memberModel.IdMember;
                MemberEditModel.Pseudo = memberModel.Pseudo;
                MemberEditModel.Email = memberModel.Email;
                MemberEditModel.Password = memberModel.PswdHash;
                MemberEditModel.Firstname = memberModel.Firstname;
                MemberEditModel.Lastname = memberModel.Lastname;
            }
        }
        private async Task SubmitEdit()
        {
            isPseudoTaken = false;
            isEmailTaken = false;
            hasMemberBeenEdited = false;
            MemberModel memberModel = new MemberModel()
            {
                IdMember = MemberEditModel.IdMember,
                Pseudo = MemberEditModel.Pseudo,
                Email = MemberEditModel.Email,
                PswdHash = MemberEditModel.Password,
                Firstname = MemberEditModel.Firstname,
                Lastname = MemberEditModel.Lastname
            };
            HttpResponseMessage response = await Http.PutAsJsonAsync($"https://localhost:7051/api/Member/{TargetId}", memberModel);

            if (!response.IsSuccessStatusCode)
            {
                string errorMessage = await response.Content.ReadAsStringAsync();
                if (errorMessage.Contains("Violation of UNIQUE KEY constraint 'UK_Member__Pseudo'."))
                    isPseudoTaken = true;
                else if (errorMessage.Contains("Violation of UNIQUE KEY constraint 'UK_Member__Email'."))
                    isEmailTaken = true;
            }
            else
            {
                hasMemberBeenEdited = true;
                await OnClose.InvokeAsync();
            }
        }
    }
}
