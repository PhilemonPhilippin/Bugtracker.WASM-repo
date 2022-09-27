using Bugtracker.WASM.Tools;
using Microsoft.AspNetCore.Components;

namespace Bugtracker.WASM.Pages
{
    public partial class MyAccount : ComponentBase
    {
        [Inject]
        private IMemberLocalStorage LocalStorage { get; set; } = default!;
        private bool _displayLogin = true;
        private bool _displayRegistration;
        private bool _isMemberConnected;

        protected async override Task OnInitializedAsync()
        {
            _isMemberConnected = await LocalStorage.HasToken();
        }
        private void DisplayLogin()
        {
            _displayLogin = !_displayLogin;
            if (_displayLogin)
                _displayRegistration = false;
        }
        private void DisplayRegistration()
        {
            _displayRegistration = !_displayRegistration;
            if (_displayRegistration)
                _displayLogin = false;
        }
    }
}
