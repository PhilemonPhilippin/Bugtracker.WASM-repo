using Bugtracker.WASM.Models;
using Bugtracker.WASM.Pages.ProjectComponents;
using static System.Net.WebRequestMethods;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components;
using Bugtracker.WASM.Tools;
using Bugtracker.WASM.Mappers;
using System.Net.Http.Json;
using Bugtracker.WASM.Models.MemberModels;
using System.Collections.Generic;

namespace Bugtracker.WASM.Pages.TicketComponents
{
    public partial class EditTicket
    {
        [Inject]
        private IMemberLocalStorage LocalStorage { get; set; } = default!;
        [Inject]
        private IApiRequester Requester { get; set; } = default!;
        [Parameter]
        public EventCallback OnCancel { get; set; }
        [Parameter]
        public EventCallback OnConfirm { get; set; }
        [Parameter]
        public TicketModel TicketTarget { get; set; }
        private List<ProjectModel> _projects = new List<ProjectModel>();
        private TicketEditModel EditedTicket { get; set; } = new TicketEditModel();
        private string _token;
        private bool _isMemberConnected;
        private bool _displayTitleTaken;

        protected override async Task OnInitializedAsync()
        {
            EditedTicket = TicketTarget.ToEditModel();

            _token = await LocalStorage.GetToken();
            if (_token is not null)
            {
                _isMemberConnected = true;
                _projects = await Requester.Get<List<ProjectModel>>("Project", _token);
            }
        }
        private async Task SubmitEdit()
        {
            _token = await LocalStorage.GetToken();
            if (_token is not null)
            {
                _isMemberConnected = true;
                _displayTitleTaken = false;
                TicketModel ticketModel = EditedTicket.ToModel();
                
                using HttpResponseMessage response = await Requester.Put(ticketModel, "Ticket", _token);
                if (!response.IsSuccessStatusCode)
                {
                    string message = await response.Content.ReadAsStringAsync();
                    if (message.Contains("Title already exists."))
                        _displayTitleTaken = true;
                }
                else
                    await OnConfirm.InvokeAsync();
            }
        }
    }
}
