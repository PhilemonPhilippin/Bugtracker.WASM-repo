using Bugtracker.WASM.Models;
using Bugtracker.WASM.Tools;
using Microsoft.AspNetCore.Components;

namespace Bugtracker.WASM.Shared.TicketComponents
{
    public partial class MyTicketList
    {
        private bool _refreshOnEdit = true;
        public void OnEdit()
        {
            // I want to refresh the whole component if any ticket was edited
            _refreshOnEdit = false;
            StateHasChanged();
            _refreshOnEdit = true;
        }
    }
}

