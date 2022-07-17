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
        private List<ProjectModel> _projects = new List<ProjectModel>();
        private string _token;
        private bool _isMemberConnected;
        private bool _displayTitleTaken;
        private bool _isTicketAdded;

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
                _projects = await Http.GetFromJsonAsync<List<ProjectModel>>("https://localhost:7051/api/Project");
            }
        }

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
                _isTicketAdded = false;
                // TODO : get ID of myself from API request asking to extract payload of my JWT Token
                Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
                AddedTicket.SubmitMember = await Http.GetFromJsonAsync<int>("https://localhost:7051/api/Member/idfromjwt");
                TicketModel ticketModel = AddedTicket.ToModel();

                HttpResponseMessage response = await Http.PostAsJsonAsync("https://localhost:7051/api/Ticket", ticketModel);

                if (!response.IsSuccessStatusCode)
                {
                    string message = await response.Content.ReadAsStringAsync();
                    if (message.Contains("Title already exists."))
                        _displayTitleTaken = true;
                }
                else
                {
                    _isTicketAdded = true;
                    await OnConfirm.InvokeAsync();
                    AddedTicket = new TicketFormModel();
                }
            }
        }
    }
}
