using Bugtracker.WASM.Mappers;
using Bugtracker.WASM.Models;
using Bugtracker.WASM.Tools;
using Microsoft.AspNetCore.Components;
using static System.Net.WebRequestMethods;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Bugtracker.WASM.Pages.TicketComponents
{
    public partial class AddTicket
    {
        [Inject]
        private HttpClient Http { get; set; }
        [Inject]
        private IMemberLocalStorage LocalStorage { get; set; }
        [Parameter]
        public EventCallback OnCancel { get; set; }
        [Parameter]
        public EventCallback OnConfirm { get; set; }
        private TicketFormModel AddedTicket { get; set; } = new TicketFormModel() { IdTicket = default, SubmitTime = DateTime.Now };
        private string _token;
        private bool _isMemberConnected;
        private bool _displayTitleTaken;

        private async Task SubmitAdd()
        {
            _token = await LocalStorage.GetToken();
            if (_token is null)
                _isMemberConnected = false;
            else
                _isMemberConnected = true;

            if (_isMemberConnected)
            {
                _displayTitleTaken = false;

                TicketModel ticketModel = AddedTicket.ToModel();

                Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
                HttpResponseMessage response = await Http.PostAsJsonAsync("https://localhost:7051/api/Ticket", ticketModel);

                if (!response.IsSuccessStatusCode)
                {
                    string message = await response.Content.ReadAsStringAsync();
                    if (message.Contains("Title already exists."))
                        _displayTitleTaken = true;
                }
            }
            else
            {
                await OnConfirm.InvokeAsync();
                AddedTicket = new TicketFormModel();
            }
        }
    }
}
