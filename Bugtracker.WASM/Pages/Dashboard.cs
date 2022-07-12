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

        private string _token;
        private bool _isMemberConnected;
        private bool _displayMembersComponent;

        protected override async Task OnInitializedAsync()
        {
            _token = await LocalStorage.GetToken();
            if (_token is null)
            {
                _isMemberConnected = false;
                ToAccount();
            }
            else
                _isMemberConnected = true;

        }
        private void ToAccount()
        {
            NavManager.NavigateTo("/account");
        }
        private void DisplayMembersComponent()
        {
            if (_displayMembersComponent)
                _displayMembersComponent = false;
            else
                _displayMembersComponent = true;
        }
    }
}
