using Bugtracker.WASM.Tools;
using Microsoft.AspNetCore.Components;

namespace Bugtracker.WASM.Pages
{
    public partial class Members
    {
        [Inject]
        NavigationManager NavManager { get; set; }
        [Inject]
        private IMemberLocalStorage LocalStorage { get; set; }
        private bool _isMemberConnected;
        private string _token;
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
