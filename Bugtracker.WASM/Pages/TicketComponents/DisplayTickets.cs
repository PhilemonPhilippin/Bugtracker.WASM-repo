﻿using Bugtracker.WASM.Models;
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
        private TicketModel _ticketTarget;
        private bool _isMemberConnected;
        private bool _displayTicketDetailsDialog;
        private bool _displayAddTicketDialog;
        private string _token;

        protected override async Task OnInitializedAsync()
        {
            _token = await LocalStorage.GetToken();
            if (_token is null)
                _isMemberConnected = false;
            else
                _isMemberConnected = true;

            if (_isMemberConnected)
            {
                Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
                _tickets = await Http.GetFromJsonAsync<List<TicketModel>>("https://localhost:7051/api/Ticket");
            }
        }
        private async Task DeleteTicket(int id)
        {
            await Http.DeleteAsync($"https://localhost:7051/api/Ticket/{id}");
            await RefreshTicketsList();
        }
        private async Task RefreshTicketsList()
        {
            _token = await LocalStorage.GetToken();
            if (_token is null)
                _isMemberConnected = false;
            else
                _isMemberConnected = true;

            if (_isMemberConnected)
            {
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
                //_displayAddTicketDialog = false;
                //_displayEditTicketDialog = false;
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
                //_displayEditTicketDialog = false;
                _displayTicketDetailsDialog = false;
                _displayAddTicketDialog = true;
            }
        }
        private void CloseAddDialog()
        {
            _displayAddTicketDialog = false;
        }
    }
}
