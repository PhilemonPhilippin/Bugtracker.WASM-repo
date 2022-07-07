using Bugtracker.WASM.Mappers;
using Bugtracker.WASM.Models;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace Bugtracker.WASM.Pages.MemberComponents
{
    public partial class RegisterMember
    {
        [Inject]
        private HttpClient Http { get; set; }
        private bool _isPseudoTaken;
        private bool _isEmailTaken;
        private bool _isRegistrationValid;
        private MemberRegistrationModel MemberRegistration { get; set; } = new MemberRegistrationModel();
        private async Task SubmitRegistration()
        {
            _isRegistrationValid = false;
            _isPseudoTaken = false;
            _isEmailTaken = false;

            MemberModel memberModel = MemberRegistration.ToModel();
            HttpResponseMessage response = await Http.PostAsJsonAsync("https://localhost:7051/api/Member", memberModel);
            if (!response.IsSuccessStatusCode)
            {
                string errorMessage = await response.Content.ReadAsStringAsync();
                HandleErrorMessage(errorMessage);
            }
            else
            {
                _isRegistrationValid = true;
                MemberRegistration = new MemberRegistrationModel();
            }
        }
        private void HandleErrorMessage(string errorMessage)
        {
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
    }
}
