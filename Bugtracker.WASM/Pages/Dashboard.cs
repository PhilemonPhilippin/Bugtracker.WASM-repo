using Bugtracker.WASM.Tools;
using Microsoft.AspNetCore.Components;

namespace Bugtracker.WASM.Pages
{
    public partial class Dashboard
    {
        [Inject]
        private IMemberLocalStorage _LocalStorage { get; set; }

        private bool _isMemberConnected = false;

        protected override async Task OnInitializedAsync()
        {
            string token = await _LocalStorage.GetToken();
            if (token is null)
                _isMemberConnected = false;
            else
                _isMemberConnected = true;
        }
    }
}
