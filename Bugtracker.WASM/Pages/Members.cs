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
    }
}
