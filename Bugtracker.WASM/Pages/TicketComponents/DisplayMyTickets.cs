﻿using Bugtracker.WASM.Models;
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
        private IMemberLocalStorage LocalStorage { get; set; } = default!;
        [Inject]
        private IApiRequester Requester { get; set; } = default!;
        private List<TicketModel> _tickets = new List<TicketModel>();
        private List<ProjectModel> _projects = new List<ProjectModel>();
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
            _isMemberConnected = await LocalStorage.HasToken();
            if (_isMemberConnected)
            {
                _token = await LocalStorage.GetToken();
                _tickets = await Requester.Get<List<TicketModel>>("Ticket", _token);
                _projects = await Requester.Get<List<ProjectModel>>("Project", _token);
                _myMemberId = await Requester.Get<int>("Member/idfromjwt", _token);
                _myTickets = _tickets.Where(t => t.AssignedMember == _myMemberId).ToList();
            }
        }

        private async Task RefreshTicketsList()
        {
            _isMemberConnected = await LocalStorage.HasToken();
            if (_isMemberConnected)
            {
                _token = await LocalStorage.GetToken();
                _tickets = await Requester.Get<List<TicketModel>>("Ticket", _token);
                _myTickets = _tickets.Where(t => t.AssignedMember == _myMemberId).ToList();
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
    }
}
