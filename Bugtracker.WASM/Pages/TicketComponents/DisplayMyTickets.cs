using Bugtracker.WASM.Models;
using static System.Net.WebRequestMethods;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components;
using Bugtracker.WASM.Tools;
using System.Net.Http.Json;

namespace Bugtracker.WASM.Pages.TicketComponents
{
    public partial class DisplayMyTickets
    {
        [Inject]
        HttpClient Http { get; set; }
        [Inject]
        IMemberLocalStorage LocalStorage { get; set; }
        private List<TicketModel> _tickets = new List<TicketModel>();
        private List<ProjectModel> _projects = new List<ProjectModel>();
        private List<TicketModel> _myTickets = new List<TicketModel>();
        private TicketModel _ticketTarget = new TicketModel() { IdTicket = 0 };
        private int _statusTarget;
        private int? _myMemberId;
        private string _token;
        private bool _isMemberConnected;
        private bool _displayTicketDetailsDialog;
        private bool _displayEditTicketDialog;

        protected override async Task OnInitializedAsync()
        {
            _isMemberConnected = await LocalStorage.HasToken();
            if (_isMemberConnected)
            {
                _token = await LocalStorage.GetToken();
                Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
                _tickets = await Http.GetFromJsonAsync<List<TicketModel>>("https://localhost:7051/api/Ticket");
                _projects = await Http.GetFromJsonAsync<List<ProjectModel>>("https://localhost:7051/api/Project");
                _myMemberId = await Http.GetFromJsonAsync<int?>("https://localhost:7051/api/Member/idfromjwt");
                _myTickets = _tickets.Where(t => t.AssignedMember == _myMemberId).ToList();
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
                _myTickets = _tickets.Where(t => t.AssignedMember == _myMemberId).ToList();
            }
        }
        private void DisplayTicketDetailsDialog(TicketModel ticket, int status)
        {
            if (_displayTicketDetailsDialog)
                _displayTicketDetailsDialog = false;
            else
            {
                _displayEditTicketDialog = false;
                _displayTicketDetailsDialog = true;
                _ticketTarget = ticket;
                _statusTarget = status;
            }
        }
        private void CloseDetailsDialog()
        {
            _displayTicketDetailsDialog = false;
        }
        private void DisplayTicketEditDialog(TicketModel ticket, int status)
        {
            if (_displayEditTicketDialog)
                _displayEditTicketDialog = false;
            else
            {
                _displayTicketDetailsDialog = false;
                _displayEditTicketDialog = true;
                _ticketTarget = ticket;
                _statusTarget = status;
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
