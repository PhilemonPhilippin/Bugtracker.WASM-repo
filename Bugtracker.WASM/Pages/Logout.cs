using Bugtracker.WASM.Tools;
using Microsoft.AspNetCore.Components;

namespace Bugtracker.WASM.Pages
{
    public partial class Logout
    {
        [Inject]
        private IMemberLocalStorage LocalStorage { get; set; }
        [Inject]
        private NavigationManager NavManager { get; set; }
        bool _isMemberConnected = true;


        public async Task ConfirmLogout()
        {
            await LocalStorage.RemoveToken();
            _isMemberConnected = false;
        }
        public void ToDashboard()
        {
            NavManager.NavigateTo("dashboard");
        }
    }
}
