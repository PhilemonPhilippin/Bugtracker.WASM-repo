﻿using Bugtracker.WASM.Tools;
using Microsoft.AspNetCore.Components;

namespace Bugtracker.WASM.Pages
{
    public partial class Logout : ComponentBase
    {
        [Inject]
        private IMemberLocalStorage LocalStorage { get; set; } = default!;
        [Inject]
        private NavigationManager NavManager { get; set; } = default!;
        private bool _isMemberConnected;

        protected override async Task OnInitializedAsync()
        {
            _isMemberConnected = await LocalStorage.HasToken();
            if (!_isMemberConnected)
                NavManager.NavigateTo("/account");
        }
        private async Task ConfirmLogout()
        {
            await LocalStorage.RemoveToken();
            _isMemberConnected = false;
            NavManager.NavigateTo("/account");
        }
        private void ToDashboard()
        {
            NavManager.NavigateTo("/");
        }
    }
}
