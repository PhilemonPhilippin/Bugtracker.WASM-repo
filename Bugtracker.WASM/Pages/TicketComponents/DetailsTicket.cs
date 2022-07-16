using Bugtracker.WASM.Models;
using Microsoft.AspNetCore.Components;

namespace Bugtracker.WASM.Pages.TicketComponents
{
    public partial class DetailsTicket
    {
        [Parameter]
        public TicketModel TicketTarget { get; set; }
        [Parameter]
        public EventCallback OnCancel { get; set; }
    }
}
