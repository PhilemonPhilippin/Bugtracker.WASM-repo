using Bugtracker.WASM.Tools;
using Microsoft.AspNetCore.Components;

namespace Bugtracker.WASM.Pages.MemberComponents
{
    public partial class LogoutMember
    {
        [Inject]
        private IMemberLocalStorage LocalStorage { get; set; }
        [Inject]
        private NavigationManager NavManager { get; set; }
        [Parameter]
        public EventCallback OnConfirm { get; set; }
        private async Task ConfirmLogout()
        {
            await LocalStorage.RemoveToken();
            await OnConfirm.InvokeAsync();
        }
        private void ToDashboard()
        {
            NavManager.NavigateTo("/");
        }
        
    }
}
