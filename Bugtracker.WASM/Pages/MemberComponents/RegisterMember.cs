using Bugtracker.WASM.Mappers;
using Bugtracker.WASM.Models.MemberModels;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace Bugtracker.WASM.Pages.MemberComponents
{
    public partial class RegisterMember
    {
        [Inject]
        private HttpClient Http { get; set; }
        private bool _displayPseudoTaken;
        private bool _displayEmailTaken;
        private bool _isRegistrationValid;
        private MemberRegistrationModel MemberRegistration { get; set; } = new MemberRegistrationModel();
        private async Task SubmitRegistration()
        {
            _isRegistrationValid = false;
            _displayPseudoTaken = false;
            _displayEmailTaken = false;

            MemberPostModel postModel = MemberRegistration.ToPostModel();
            using HttpResponseMessage response = await Http.PostAsJsonAsync("https://localhost:7051/api/Member", postModel);
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
                _displayPseudoTaken = true;
                _displayEmailTaken = true;
            }

            else if (errorMessage.Contains("Pseudo already exists."))
                _displayPseudoTaken = true;

            else if (errorMessage.Contains("Email already exists."))
                _displayEmailTaken = true;
        }
    }
}
