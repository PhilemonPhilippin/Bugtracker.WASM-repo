using Bugtracker.WASM.Models.MemberModels;
using Bugtracker.WASM.Tools;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Net.Http.Json;

namespace Bugtracker.WASM.Pages.MemberComponents
{
    public partial class LoginMember
    {
        [Inject]
        private HttpClient Http { get; set; }
        [Inject]
        private NavigationManager NavManager { get; set; }
        [Inject]
        private IMemberLocalStorage LocalStorage { get; set; }
        private MemberLoginModel MemberLogin { get; set; } = new MemberLoginModel();
        //private ConnectedMemberModel ConnectedMember { get; set; } = new ConnectedMemberModel();
        private bool _displayPseudoNotFound;
        private bool _displayIncorrectPassword;
        private bool _displayMemberDisabled;
        private async Task SubmitLogin()
        {
            _displayMemberDisabled = false;
            _displayPseudoNotFound = false;
            _displayIncorrectPassword = false;
            using HttpResponseMessage response = await Http.PostAsJsonAsync("https://localhost:7051/api/Member/login", MemberLogin);
            if (!response.IsSuccessStatusCode)
            {
                string errorMessage = await response.Content.ReadAsStringAsync();
                if (errorMessage.Contains("Pseudo not found."))
                    _displayPseudoNotFound = true;
                else if (errorMessage.Contains("Password incorrect."))
                    _displayIncorrectPassword = true;
                else if (errorMessage.Contains("Member has been disabled."))
                    _displayMemberDisabled = true;
            }
            else
            {
                string token = await response.Content.ReadAsStringAsync();
                await LocalStorage.SetToken(token);
                NavManager.NavigateTo("/");
            }
        }
    }
}
