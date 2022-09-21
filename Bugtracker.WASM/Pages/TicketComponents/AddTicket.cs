using Bugtracker.WASM.Mappers;
using Bugtracker.WASM.Models;
using Bugtracker.WASM.Tools;
using Microsoft.AspNetCore.Components;

namespace Bugtracker.WASM.Pages.TicketComponents
{
    public partial class AddTicket
    {
        [Inject]
        private IMemberLocalStorage LocalStorage { get; set; } = default!;
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
            _token = await LocalStorage.GetToken();
            if (_token is not null)
            {
                _isMemberConnected = true;
                _projects = await Requester.Get<List<ProjectModel>>("Project", _token);
            }
        }
        private async Task SubmitAdd()
        {
            _token = await LocalStorage.GetToken();
            if (_token is not null)
            {
                _isMemberConnected = true;
                _displayTitleTaken = false;
                _isTicketAdded = false;

                int submitMemberId = await Requester.Get<int>("Member/idfromjwt", _token);

                TicketModel ticketModel = AddedTicket.ToModel();
                ticketModel.SubmitMember = submitMemberId;

                using HttpResponseMessage response = await Requester.Post(ticketModel, "Ticket", _token);

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
