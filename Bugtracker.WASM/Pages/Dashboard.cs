using Bugtracker.WASM.Tools;
using Microsoft.AspNetCore.Components;
using static System.Net.WebRequestMethods;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Bugtracker.WASM.Models;

namespace Bugtracker.WASM.Pages
{
    public partial class Dashboard
    {
        [Inject]
        private IMemberLocalStorage LocalStorage { get; set; }
        [Inject]
        private NavigationManager NavManager { get; set; }

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
