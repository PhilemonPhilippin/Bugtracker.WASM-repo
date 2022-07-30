using Bugtracker.WASM.Tools;
using Microsoft.AspNetCore.Components;

namespace Bugtracker.WASM.Pages
{
    public partial class ProjectManager
    {
        [Inject]
        NavigationManager NavManager { get; set; }
        [Inject]
        IMemberLocalStorage LocalStorage { get; set; }
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
            if (_displayProjectsComponent)
                _displayProjectsComponent = false;
            else
            {
                _displayTicketsComponent = false;
                _displayProjectsComponent = true;
            }
        }
        private void DisplayTicketsComponent()
        {
            if (_displayTicketsComponent)
                _displayTicketsComponent = false;
            else
            {
                _displayProjectsComponent = false;
                _displayTicketsComponent = true;
            }
        }
    }
}
