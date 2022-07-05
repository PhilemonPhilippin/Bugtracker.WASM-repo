using Bugtracker.WASM.Models;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace Bugtracker.WASM.Pages.MemberComponents
{
    public partial class RegisterMember
    {
        [Inject]
        public HttpClient Http { get; set; }
        [Inject]
        public NavigationManager NavManager { get; set; }
        private bool _isPseudoTaken = false;
        private bool _isEmailTaken = false;
        private MemberRegistrationModel _MemberRegistrationModel { get; set; } = new MemberRegistrationModel();
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
                if (errorMessage.Contains("Pseudo and Email already exist."))
                {
                    _isPseudoTaken = true;
                    _isEmailTaken = true;
                }
                else if (errorMessage.Contains("Pseudo already exists."))
                    _isPseudoTaken = true;
                else if (errorMessage.Contains("Email already exists."))
                    _isEmailTaken = true;
            }
            else
                NavManager.NavigateTo("dashboard");
        }
    }
}
