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
        private int? _myMemberId;
        private string _token;
        private bool _isMemberConnected;
        private bool _displayTicketDetailsDialog;

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

        private void DisplayTicketDetailsDialog(TicketModel ticket)
        {
            if (_displayTicketDetailsDialog)
                _displayTicketDetailsDialog = false;
            else
            {
                _displayTicketDetailsDialog = true;
                _ticketTarget = ticket;
            }
        }
        private void CloseDetailsDialog()
        {
            _displayTicketDetailsDialog = false;
        }
    }
}
