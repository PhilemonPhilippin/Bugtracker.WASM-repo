using Bugtracker.WASM.Tools;
using Microsoft.AspNetCore.Components;

namespace Bugtracker.WASM.Pages
{
    public partial class Dashboard
    {
        [Inject]
        private IMemberLocalStorage LocalStorage { get; set; } = default!;
        [Inject]
        private NavigationManager NavManager { get; set; } = default!;

        private bool _isMemberConnected;
        private bool _displayMyTicketsComponent = true;
        private bool _displayMyProjectsComponent;
        protected override async Task OnInitializedAsync()
        {
            _isMemberConnected = await LocalStorage.HasToken();
            if (!_isMemberConnected)
                NavManager.NavigateTo("/account");
        }
        private void ToAccount()
        {
            NavManager.NavigateTo("/account");
        }

        private void DisplayMyTicketsComponent()
        {
            _displayMyTicketsComponent = !_displayMyTicketsComponent;
            if (_displayMyTicketsComponent)
                _displayMyProjectsComponent = false;
        }
        private void DisplayMyProjectsComponent()
        {
            _displayMyProjectsComponent = !_displayMyProjectsComponent;
            if (_displayMyProjectsComponent)
                _displayMyTicketsComponent = false;
        }
    }
}
