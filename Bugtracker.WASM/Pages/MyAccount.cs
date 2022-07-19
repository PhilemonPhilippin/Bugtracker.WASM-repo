using Bugtracker.WASM.Tools;
using Microsoft.AspNetCore.Components;

namespace Bugtracker.WASM.Pages
{
    public partial class MyAccount
    {
        [Inject]
        private IMemberLocalStorage LocalStorage { get; set; }
        //[Inject]
        //private NavigationManager NavManager { get; set; }
        private bool _displayLogin;
        private bool _displayRegistration;
        private bool _isMemberConnected;
        protected async override Task OnInitializedAsync()
        {
            _isMemberConnected = await LocalStorage.HasToken();
        }
        private void DisplayLogin()
        {
            _displayRegistration = false;
            _displayLogin = true;
        }
        private void DisplayRegistration()
        {
            _displayLogin = false;
            _displayRegistration = true;
        }
    }
}
