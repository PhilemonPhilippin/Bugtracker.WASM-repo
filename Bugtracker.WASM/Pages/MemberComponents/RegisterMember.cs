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
            // if I ever need to get back the member that I posted :
            // MemberRegistrationVm responseMember = await response.Content.ReadFromJsonAsync<MemberRegistrationVm>();
            //object? responseObject = await response.Content.ReadFromJsonAsync<object?>();
            if (!response.IsSuccessStatusCode)
            {

            }
            else
                navManager.NavigateTo("dashboard");
        }
    }
}
