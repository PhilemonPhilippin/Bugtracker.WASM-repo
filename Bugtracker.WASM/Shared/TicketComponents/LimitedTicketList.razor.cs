using Bugtracker.WASM.Models;
using Bugtracker.WASM.Models.MemberModels;
using Bugtracker.WASM.Tools;
using Microsoft.AspNetCore.Components;

namespace Bugtracker.WASM.Shared.TicketComponents
{
    public partial class LimitedTicketList
    {
        [Inject]
        private IMemberLocalStorage LocalStorage { get; set; } = default!;
        [Inject]
        private IApiRequester Requester { get; set; } = default!;
        private List<TicketModel> _tickets = new List<TicketModel>();
        private List<ProjectModel> _projects = new List<ProjectModel>();
        private List<int> _disabledProjects = new List<int>();
        private TicketModel _ticketTarget = new TicketModel() { IdTicket = 0 };
        private string _token;
        private bool _isMemberConnected;
        private bool _displayDetailsDialog;
        private bool _displayAddDialog;
        private int _myMemberId;
        private MemberModel _myMemberModel = new();
        private bool _displayEditDialog;
        private bool _displayAssignDialog;
        protected override async Task OnInitializedAsync()
        {
            _token = await LocalStorage.GetToken();
            if (_token is not null)
            {
                _isMemberConnected = true;
                _myMemberId = await Requester.Get<int>("Member/idfromjwt", _token);
                _myMemberModel = await Requester.Get<MemberModel>($"Member/{_myMemberId}", _token);

                _tickets = await Requester.Get<List<TicketModel>>("Ticket", _token);
                _projects = await Requester.Get<List<ProjectModel>>("Project", _token);
                // Je veux filtrer les tickets dont les projets sont désactivés :
                _disabledProjects = _projects.Where(p => p.Disabled).Select(p => p.IdProject).ToList();
                _tickets = _tickets.Where(t => _disabledProjects.Contains(t.Project) == false).ToList();
            }
        }
        private async Task RefreshTicketList()
        {
            _token = await LocalStorage.GetToken();
            if (_token is not null)
            {
                _isMemberConnected = true;
                _tickets = await Requester.Get<List<TicketModel>>("Ticket", _token);
                _tickets = _tickets.Where(t => _disabledProjects.Contains(t.Project) == false).ToList();
            }
        }
        private async Task ConfirmEdit()
        {
            await RefreshTicketList();
            _displayEditDialog = false;
        }
        private async Task ConfirmAssign()
        {
            await RefreshTicketList();
            _displayAssignDialog = false;
        }
        private void DisplayDetailsDialog(TicketModel ticket)
        {
            _displayDetailsDialog = !_displayDetailsDialog;
            if (_displayDetailsDialog)
            {
                _displayAddDialog = false;
                _displayEditDialog = false;
                _displayAssignDialog = false;
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
                _displayAssignDialog = false;
            }
        }
        private void DisplayEditDialog(TicketModel ticket)
        {
            _displayEditDialog = !_displayEditDialog;
            if (_displayEditDialog)
            {
                _displayAddDialog = false;
                _displayDetailsDialog = false;
                _displayAssignDialog = false;
                _ticketTarget = ticket;
            }
        }

        private void DisplayAssignDialog(TicketModel ticket)
        {
            _displayAssignDialog = !_displayAssignDialog;
            if (_displayAssignDialog)
            {
                _displayEditDialog = false;
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
        private void CloseAssignDialog()
        {
            _displayAssignDialog = false;
        }
    }
}
