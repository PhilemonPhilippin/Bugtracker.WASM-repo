using Bugtracker.WASM.Models;
using Bugtracker.WASM.Tools;
using Microsoft.AspNetCore.Components;

namespace Bugtracker.WASM.Shared.TicketComponents
{
    public partial class MyTicketListPerPriority
    {
        [Inject]
        private IMemberLocalStorage LocalStorage { get; set; } = default!;
        [Inject]
        private IApiRequester Requester { get; set; } = default!;
        [Parameter]
        public int PriorityNumber { get; set; }
        private List<TicketModel> _tickets = new List<TicketModel>();
        private List<ProjectModel> _projects = new List<ProjectModel>();
        private List<int> _disabledProjects = new List<int>();
        private List<TicketModel> _myTickets = new List<TicketModel>();
        private TicketModel _ticketTarget = new TicketModel() { IdTicket = 0 };
        private int _statusTarget;
        private int _myMemberId;
        private string _token;
        private bool _isMemberConnected;
        private bool _displayTicketDetailsDialog;
        private bool _displayEditTicketDialog;

        protected override async Task OnInitializedAsync()
        {
            _token = await LocalStorage.GetToken();
            if (_token is not null)
            {
                _isMemberConnected = true;
                _tickets = await Requester.Get<List<TicketModel>>("Ticket", _token);
                _projects = await Requester.Get<List<ProjectModel>>("Project", _token);
                _myMemberId = await Requester.Get<int>("Member/idfromjwt", _token);
                _myTickets = _tickets.Where(t => t.AssignedMember == _myMemberId).ToList();
                // Je veux filtrer les tickets dont les projets sont désactivés :
                _disabledProjects = _projects.Where(p => p.Disabled).Select(p => p.IdProject).ToList();
                _myTickets = _myTickets.Where(t => _disabledProjects.Contains(t.Project) == false).ToList();
            }
        }

        private async Task RefreshTicketsList()
        {
            _token = await LocalStorage.GetToken();
            if (_token is not null)
            {
                _isMemberConnected = true;
                _tickets = await Requester.Get<List<TicketModel>>("Ticket", _token);
                _myTickets = _tickets.Where(t => t.AssignedMember == _myMemberId).ToList();
                _myTickets = _myTickets.Where(t => _disabledProjects.Contains(t.Project) == false).ToList();
            }
        }
        private async Task ConfirmTicketEdit()
        {
            await RefreshTicketsList();
            _displayEditTicketDialog = false;
        }
        private void DisplayTicketDetailsDialog(TicketModel ticket, int status)
        {
            _displayTicketDetailsDialog = !_displayTicketDetailsDialog;
            if (_displayTicketDetailsDialog)
            {
                _displayEditTicketDialog = false;
                _ticketTarget = ticket;
                _statusTarget = status;
            }
        }
        private void DisplayTicketEditDialog(TicketModel ticket, int status)
        {
            _displayEditTicketDialog = !_displayEditTicketDialog;
            if (_displayEditTicketDialog)
            {
                _displayTicketDetailsDialog = false;
                _ticketTarget = ticket;
                _statusTarget = status;
            }
        }
        private void CloseEditDialog()
        {
            _displayEditTicketDialog = false;
        }
        private void CloseDetailsDialog()
        {
            _displayTicketDetailsDialog = false;
        }
        private string PriorityName(int priorityNumber)
        {
            switch (priorityNumber)
            {
                case 1:
                    return "To-do";
                case 2:
                    return "In progress";
                case 3:
                    return "Stuck";
                default:
                    return "Complete";
            }
        }
        private string PriorityTableColor(int priorityNumber)
        {
            switch (priorityNumber)
            {
                case 1:
                case 2:
                    return "primary";
                case 3:
                    return "warning";
                default:
                    return "success";
            }
        }
    }
}
