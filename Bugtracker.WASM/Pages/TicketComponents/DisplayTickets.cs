using Bugtracker.WASM.Models;
using Bugtracker.WASM.Tools;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Bugtracker.WASM.Pages.TicketComponents
{
    public partial class DisplayTickets
    {
        [Inject]
        private IMemberLocalStorage LocalStorage { get; set; } = default!;
        [Inject]
        private IApiRequester Requester { get; set; } = default!;
        private List<TicketModel> _tickets = new List<TicketModel>();
        private List<ProjectModel> _projects = new List<ProjectModel>();
        private TicketModel _ticketTarget = new TicketModel() { IdTicket = 0 };
        private string _token;
        private bool _isMemberConnected;
        private bool _displayDetailsDialog;
        private bool _displayAddDialog;
        private bool _displayEditDialog;
        protected override async Task OnInitializedAsync()
        {
            _isMemberConnected = await LocalStorage.HasToken();
            if (_isMemberConnected)
            {
                _token = await LocalStorage.GetToken();
                _tickets = await Requester.Get<List<TicketModel>>("Ticket", _token);
                _projects = await Requester.Get<List<ProjectModel>>("Project", _token);
            }
        }
        private async Task DeleteTicket(TicketModel ticket)
        {
            _isMemberConnected = await LocalStorage.HasToken();
            if (_isMemberConnected)
            {
                _token = await LocalStorage.GetToken();
                // Si il n'existe qu'un seul ticket pour lequel l'assign entre ticket et project existe, on le delete.
                if (ticket.AssignedMember is not null)
                {
                    int assignedMemberId = (int)ticket.AssignedMember;
                    if (_tickets.Count(t => t.AssignedMember == ticket.AssignedMember && t.Project == ticket.Project) == 1)
                    {
                        await Requester.Delete($"Assign/{ticket.Project}/{assignedMemberId}", _token);
                    }
                }
                await Requester.Delete($"Ticket/{ticket.IdTicket}", _token);
                await RefreshTicketList();
            }
        }
        private async Task RefreshTicketList()
        {
            _isMemberConnected = await LocalStorage.HasToken();
            if (_isMemberConnected)
            {
                _token = await LocalStorage.GetToken();
                _tickets = await Requester.Get<List<TicketModel>>("Ticket", _token);
            }
        }
        private async Task ConfirmEdit()
        {
            await RefreshTicketList();
            _displayEditDialog = false;
        }
        private void DisplayDetailsDialog(TicketModel ticket)
        {
            _displayDetailsDialog = !_displayDetailsDialog;
            if (_displayDetailsDialog)
            {
                _displayAddDialog = false;
                _displayEditDialog = false;
                _ticketTarget = ticket;
            }
        }
        private void DisplayAddDialog()
        {
            _displayAddDialog = !_displayAddDialog;
            if (_displayAddDialog)
            {
                _displayEditDialog = false;
                _displayDetailsDialog = false;
            }
        }
        private void DisplayEditDialog(TicketModel ticket)
        {
            _displayEditDialog = !_displayEditDialog;
            if (_displayEditDialog)
            {
                _displayAddDialog = false;
                _displayDetailsDialog = false;
                _ticketTarget = ticket;
            }
        }
        private void CloseDetailsDialog()
        {
            _displayDetailsDialog = false;
        }
        private void CloseAddDialog()
        {
            _displayAddDialog = false;
        }
        private void CloseEditDialog()
        {
            _displayEditDialog = false;
        }
    }
}
