using Bugtracker.WASM.Models;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace Bugtracker.WASM.Pages.MemberComponents
{
    public partial class RegisterMember
    {
        [Inject]
        public HttpClient Http { get; set; }
        private bool _isPseudoTaken = false;
        private bool _isEmailTaken = false;
        private MemberRegistrationModel _MemberRegistrationModel;
        public RegisterMember()
        {
            _MemberRegistrationModel = new MemberRegistrationModel();
        }
        private async Task SubmitRegistration()
        {
            _isPseudoTaken = false;
            _isEmailTaken = false;
            MemberModel memberModel = new MemberModel()
            {
                IdMember = 0,
                Pseudo = _MemberRegistrationModel.Pseudo,
                Email = _MemberRegistrationModel.Email,
                PswdHash = _MemberRegistrationModel.Password,
                Firstname = _MemberRegistrationModel.Firstname,
                Lastname = _MemberRegistrationModel.Lastname
            };
            HttpResponseMessage response = await Http.PostAsJsonAsync("https://localhost:7051/api/Member", memberModel);
            if (!response.IsSuccessStatusCode)
            {
                string errorMessage = await response.Content.ReadAsStringAsync();
                if (errorMessage.Contains("Violation of UNIQUE KEY constraint 'UK_Member__Pseudo'."))
                    _isPseudoTaken = true;
                else if (errorMessage.Contains("Violation of UNIQUE KEY constraint 'UK_Member__Email'."))
                    _isEmailTaken = true;
            }
            else
                navManager.NavigateTo("dashboard");
        }
    }
}
