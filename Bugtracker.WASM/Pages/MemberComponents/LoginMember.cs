using Bugtracker.WASM.Models;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace Bugtracker.WASM.Pages.MemberComponents
{
    public partial class LoginMember
    {
        [Inject]
        public HttpClient Http { get; set; }
        [Inject]
        public NavigationManager NavManager { get; set; }
        private MemberLoginModel _MemberLoginModel { get; set; } = new MemberLoginModel();
        private bool showPseudoNotFound;
        private bool showPasswordIncorrect;

        private async Task SubmitLogin()
        {
            showPseudoNotFound = false;
            showPasswordIncorrect = false;
            HttpResponseMessage response = await Http.PostAsJsonAsync("https://localhost:7051/api/Member/login", _MemberLoginModel);
            if (!response.IsSuccessStatusCode)
            {
                string errorMessage = await response.Content.ReadAsStringAsync();
                if (errorMessage.Contains("Pseudo not found."))
                    showPseudoNotFound = true;
                else if (errorMessage.Contains("Password incorrect."))
                    showPasswordIncorrect = true;
            }
            else
                NavManager.NavigateTo("dashboard");
        }
    }
}
