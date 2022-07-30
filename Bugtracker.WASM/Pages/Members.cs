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
        private bool _displayMembersComponent = true;
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
        private void DisplayMembersComponent()
        {
            if (_displayMembersComponent)
                _displayMembersComponent = false;
            else
                _displayMembersComponent = true;
        }
    }
}
