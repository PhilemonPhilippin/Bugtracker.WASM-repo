using Bugtracker.WASM.Models;
using Bugtracker.WASM.Tools;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Net.Http.Json;

namespace Bugtracker.WASM.Pages.MemberComponents
{
    public partial class LoginMember
    {
        [Inject]
        private HttpClient _Http { get; set; }
        [Inject]
        private NavigationManager _NavManager { get; set; }
        [Inject]
        private IMemberLocalStorage _LocalStorage { get; set; }
        private MemberLoginModel _MemberLoginModel { get; set; } = new MemberLoginModel();
        private ConnectedMemberModel _ConnectedMemberModel { get; set; } = new ConnectedMemberModel();
       
        private bool showPseudoNotFound;
        private bool showPasswordIncorrect;

        private async Task SubmitLogin()
        {
            showPseudoNotFound = false;
            showPasswordIncorrect = false;
            HttpResponseMessage response = await _Http.PostAsJsonAsync("https://localhost:7051/api/Member/login", _MemberLoginModel);
            if (!response.IsSuccessStatusCode)
            {
                string errorMessage = await response.Content.ReadAsStringAsync();
                if (errorMessage.Contains("Pseudo not found."))
                    showPseudoNotFound = true;
                else if (errorMessage.Contains("Password incorrect."))
                    showPasswordIncorrect = true;
            }
            else
            {
                _ConnectedMemberModel = await response.Content.ReadFromJsonAsync<ConnectedMemberModel>();
                // TODO : réussir à stoker le membre ou token dans le local storage.
                await _LocalStorage.SetToken(_ConnectedMemberModel.Token);
                //_NavManager.NavigateTo("dashboard");
            }
        }
    }
}
