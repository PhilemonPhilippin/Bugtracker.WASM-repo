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
                await RefreshTicketList();
            }
        }
        private async Task RefreshTicketList()
        {
            _isMemberConnected = await LocalStorage.HasToken();
            if (_isMemberConnected)
            {
                _token = await LocalStorage.GetToken();
                Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
                _tickets = await Http.GetFromJsonAsync<List<TicketModel>>("https://localhost:7051/api/Ticket");
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
