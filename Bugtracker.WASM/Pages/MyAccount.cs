using Bugtracker.WASM.Tools;
using Microsoft.AspNetCore.Components;

namespace Bugtracker.WASM.Pages
{
    public partial class MyAccount
    {
        [Inject]
        private IMemberLocalStorage LocalStorage { get; set; }
        [Inject]
        private NavigationManager NavManager { get; set; }

        private bool _displayLogin;
        private bool _displayRegistration;
        private bool _isMemberConnected;
        private string _token;

        protected async override Task OnInitializedAsync()
        {
            _token = await LocalStorage.GetToken();
            if (_token is null)
                _isMemberConnected = false;
            else
                _isMemberConnected = true;
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
