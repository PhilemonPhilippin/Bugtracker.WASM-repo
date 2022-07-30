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
        public HttpClient Http { get; set; }
        [Inject]
        IMemberLocalStorage LocalStorage { get; set; }

        private List<TicketModel> _tickets = new List<TicketModel>();
        private List<ProjectModel> _projects = new List<ProjectModel>();
        private TicketModel _ticketTarget = new TicketModel() { IdTicket = 0 };
        private bool _isMemberConnected;
        private bool _displayTicketDetailsDialog;
        private bool _displayAddTicketDialog;
        private bool _displayEditTicketDialog;
        private string _token;
        protected override async Task OnInitializedAsync()
        {
            _isMemberConnected = await LocalStorage.HasToken();
            if (_isMemberConnected)
            {
                _token = await LocalStorage.GetToken();
                Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
                _tickets = await Http.GetFromJsonAsync<List<TicketModel>>("https://localhost:7051/api/Ticket");
                _projects = await Http.GetFromJsonAsync<List<ProjectModel>>("https://localhost:7051/api/Project");
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
                        Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
                        await Http.DeleteAsync($"https://localhost:7051/api/Assign/{ticket.Project}/{assignedMemberId}");
                    }
                }
                await Http.DeleteAsync($"https://localhost:7051/api/Ticket/{ticket.IdTicket}");
                await RefreshTicketsList();
            }
        }
        private async Task RefreshTicketsList()
        {
            _isMemberConnected = await LocalStorage.HasToken();
            if (_isMemberConnected)
            {
                _token = await LocalStorage.GetToken();
                Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
                _tickets = await Http.GetFromJsonAsync<List<TicketModel>>("https://localhost:7051/api/Ticket");
            }
        }
        private void DisplayTicketDetailsDialog(TicketModel ticket)
        {
            if (_displayTicketDetailsDialog)
                _displayTicketDetailsDialog = false;
            else
            {
                _displayAddTicketDialog = false;
                _displayEditTicketDialog = false;
                _displayTicketDetailsDialog = true;
                _ticketTarget = ticket;
            }
        }
        private void CloseDetailsDialog()
        {
            _displayTicketDetailsDialog = false;
        }
        private void DisplayAddTicketDialog()
        {
            if (_displayAddTicketDialog)
                _displayAddTicketDialog = false;
            else
            {
                _displayEditTicketDialog = false;
                _displayTicketDetailsDialog = false;
                _displayAddTicketDialog = true;
            }
        }
        private void CloseAddDialog()
        {
            _displayAddTicketDialog = false;
        }
        private void DisplayTicketEditDialog(TicketModel ticket)
        {
            if (_displayEditTicketDialog)
                _displayEditTicketDialog = false;
            else
            {
                _displayAddTicketDialog = false;
                _displayTicketDetailsDialog = false;
                _displayEditTicketDialog = true;
                _ticketTarget = ticket;
            }
        }
        private void CloseEditDialog()
        {
            _displayEditTicketDialog = false;
        }
        private async Task ConfirmTicketEdit()
        {
            await RefreshTicketsList();
            _displayEditTicketDialog = false;
        }
    }
}
