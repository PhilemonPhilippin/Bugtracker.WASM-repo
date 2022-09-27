using Bugtracker.WASM.Mappers;
using Bugtracker.WASM.Models;
using Bugtracker.WASM.Tools;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Bugtracker.WASM.Shared.TicketComponents
{
    public partial class TicketAdd : ComponentBase
    {
        [Inject]
        private IMemberLocalStorage LocalStorage { get; set; } = default!;
        [Inject]
        private IApiRequester Requester { get; set; } = default!;
        [Inject]
        private IJSRuntime JS { get; set; } = default!;
        [Parameter]
        public EventCallback OnCancel { get; set; }
        [Parameter]
        public EventCallback OnConfirm { get; set; }
        private TicketFormModel AddedTicket { get; set; } = new TicketFormModel() { IdTicket = default, SubmitTime = DateTime.Now };
        private List<ProjectModel> _projects = new List<ProjectModel>();
        private ElementReference focusRef;
        private IJSObjectReference? module;
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

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                module = await JS.InvokeAsync<IJSObjectReference>("import", "./js/mainscript.js");
                await module.InvokeVoidAsync("ScrollToRef", focusRef);
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
