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
        [Inject]
        private IApiRequester Requester { get; set; } = default!;
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
            _isMemberConnected = await LocalStorage.HasToken();
            if (_isMemberConnected)
            {
                _token = await LocalStorage.GetToken();
                _projects = await Requester.Get<List<ProjectModel>>("Project", _token);
            }
        }
        private async Task SubmitAdd()
        {
            _isMemberConnected = await LocalStorage.HasToken();
            if (_isMemberConnected)
            {
                _displayTitleTaken = false;
                _isTicketAdded = false;

                _token = await LocalStorage.GetToken();

                int submitMemberId = await Requester.Get<int>("Member/idfromjwt", _token);

                TicketModel ticketModel = AddedTicket.ToModel();
                ticketModel.SubmitMember = submitMemberId;

                using HttpResponseMessage response = await Http.PostAsJsonAsync("https://localhost:7051/api/Ticket", ticketModel);

                if (!response.IsSuccessStatusCode)
                {
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    if (errorMessage.Contains("Title already exists."))
                        _displayTitleTaken = true;
                }
                else
                {
                    _isTicketAdded = true;
                    await OnConfirm.InvokeAsync();
                    AddedTicket = new TicketFormModel() { IdTicket = default, SubmitTime = DateTime.Now };
                }
            }
        }
    }
}
