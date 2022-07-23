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
        private HttpClient Http { get; set; }
        [Inject]
        private IMemberLocalStorage LocalStorage { get; set; }
        [Inject]
        NavigationManager NavManager { get; set; }

        private bool _isMemberConnected;
        private bool _displayProjectsComponent;
        private bool _displayTicketsComponent;
        private bool _displayMyTicketsComponent;
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
        private void DisplayMyTicketsComponent()
        {
            if (_displayMyTicketsComponent)
                _displayMyTicketsComponent = false;
            else
            {
                _displayMyProjectsComponent = false;
                _displayMyTicketsComponent = true;
            }
        }
        private void DisplayMyProjectsComponent()
        {
            if (_displayMyProjectsComponent)
                _displayMyProjectsComponent = false;
            else
            {
                _displayMyTicketsComponent = false;
                _displayMyProjectsComponent = true;
            }
        }
    }
}
