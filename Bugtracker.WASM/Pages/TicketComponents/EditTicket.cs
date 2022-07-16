using Bugtracker.WASM.Models;
using Bugtracker.WASM.Pages.ProjectComponents;
using static System.Net.WebRequestMethods;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components;
using Bugtracker.WASM.Tools;
using Bugtracker.WASM.Mappers;
using System.Net.Http.Json;

namespace Bugtracker.WASM.Pages.TicketComponents
{
    public partial class EditTicket
    {
        [Inject]
        private HttpClient Http { get; set; }
        [Inject]
        private IMemberLocalStorage LocalStorage { get; set; }
        [Parameter]
        public EventCallback OnCancel { get; set; }
        [Parameter]
        public EventCallback OnConfirm { get; set; }
        [Parameter]
        public TicketModel TicketTarget { get; set; }
        private TicketFormModel EditedTicket { get; set; } = new TicketFormModel();
        private string _token;
        private bool _isMemberConnected;
        private bool _displayTitleTaken;

        protected override async Task OnInitializedAsync()
        {
            EditedTicket = TicketTarget.ToFormModel();
        }
        private async Task SubmitEdit()
        {
            _token = await LocalStorage.GetToken();
            if (_token is null)
                _isMemberConnected = false;
            else
                _isMemberConnected = true;
            if (_isMemberConnected)
            {
                _displayTitleTaken = false;

                TicketModel ticketModel = EditedTicket.ToModel();

                Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
                HttpResponseMessage response = await Http.PutAsJsonAsync("https://localhost:7051/api/Ticket", ticketModel);
                if (!response.IsSuccessStatusCode)
                {
                    string message = await response.Content.ReadAsStringAsync();
                    if (message.Contains("Title already exists."))
                        _displayTitleTaken = true;
                }
                else
                {
                    await OnConfirm.InvokeAsync();
                }
            }
        }

    }
}
