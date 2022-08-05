using Bugtracker.WASM.Tools;
using Microsoft.AspNetCore.Components;

namespace Bugtracker.WASM.Pages
{
    public partial class ProjectManager
    {
        [Inject]
        private NavigationManager NavManager { get; set; }
        [Inject]
        private IMemberLocalStorage LocalStorage { get; set; }
        private bool _isMemberConnected;
        private bool _displayProjectsComponent = true;
        private bool _displayTicketsComponent;

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
        private void DisplayProjectsComponent()
        {
            _displayProjectsComponent = !_displayProjectsComponent;
            if (_displayProjectsComponent)
                _displayTicketsComponent = false;
        }
        private void DisplayTicketsComponent()
        {
            _displayTicketsComponent = !_displayTicketsComponent;
            if (_displayTicketsComponent)
                _displayProjectsComponent = false;
        }
    }
}
