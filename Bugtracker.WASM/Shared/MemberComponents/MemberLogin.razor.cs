using Bugtracker.WASM.Models.MemberModels;
using Bugtracker.WASM.Tools;
using Microsoft.AspNetCore.Components;

namespace Bugtracker.WASM.Shared.MemberComponents
{
    public partial class MemberLogin : ComponentBase
    {
        [Inject]
        private NavigationManager NavManager { get; set; } = default!;
        [Inject]
        private IMemberLocalStorage LocalStorage { get; set; } = default!;
        [Inject]
        private IApiRequester Requester { get; set; } = default!;
        private MemberLoginModel _MemberLogin { get; set; } = new MemberLoginModel();
        private bool _displayPseudoNotFound;
        private bool _displayIncorrectPassword;
        private bool _displayMemberDisabled;

        private async Task SubmitLogin()
        {
            _displayMemberDisabled = false;
            _displayPseudoNotFound = false;
            _displayIncorrectPassword = false;

            using HttpResponseMessage response = await Requester.Post(_MemberLogin, "Member/login");
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
