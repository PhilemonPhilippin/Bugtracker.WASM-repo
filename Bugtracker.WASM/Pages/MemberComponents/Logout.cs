using Bugtracker.WASM.Tools;
using Microsoft.AspNetCore.Components;

namespace Bugtracker.WASM.Pages.MemberComponents
{
    public partial class Logout
    {
        [Inject]
        private IMemberLocalStorage LocalStorage { get; set; }
        [Inject]
        private NavigationManager NavManager { get; set; }
        private bool _isMemberConnected;
        private string _token;

        protected override async Task OnInitializedAsync()
        {
            _token = await LocalStorage.GetToken();
            if (_token is null)
                _isMemberConnected = false;
            else
                _isMemberConnected = true;
        }
        private async Task ConfirmLogout()
        {
            await LocalStorage.RemoveToken();
            _isMemberConnected = false;
        }
        private void ToDashboard()
        {
            NavManager.NavigateTo("dashboard");
        }
        private void ToLogin()
        {
            NavManager.NavigateTo("/");
        }
    }
}
