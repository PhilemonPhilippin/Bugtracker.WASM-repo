using Bugtracker.WASM.Models;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace Bugtracker.WASM.Pages.MemberComponents
{
    public partial class RegisterMember
    {
        [Inject]
        public HttpClient Http { get; set; }
        public bool isPseudoTaken = false;
        public bool isEmailTaken = false;
        public MemberRegistrationModel memberRegistrationModel;
        public RegisterMember()
        {
            memberRegistrationModel = new MemberRegistrationModel();
        }
        private async Task SubmitRegistration()
        {
            isPseudoTaken = false;
            isEmailTaken = false;
            MemberModel memberModel = new MemberModel()
            {
                IdMember = 0,
                Pseudo = memberRegistrationModel.Pseudo,
                Email = memberRegistrationModel.Email,
                PswdHash = memberRegistrationModel.Password,
                Firstname = memberRegistrationModel.Firstname,
                Lastname = memberRegistrationModel.Lastname
            };
            HttpResponseMessage response = await Http.PostAsJsonAsync("https://localhost:7051/api/Member", memberModel);
            if (!response.IsSuccessStatusCode)
            {
                string errorMessage = await response.Content.ReadAsStringAsync();
                if (errorMessage.Contains("Violation of UNIQUE KEY constraint 'UK_Member__Pseudo'."))
                    isPseudoTaken = true;
                else if (errorMessage.Contains("Violation of UNIQUE KEY constraint 'UK_Member__Email'."))
                    isEmailTaken = true;
            }
            else
                navManager.NavigateTo("dashboard");
        }
    }
}
