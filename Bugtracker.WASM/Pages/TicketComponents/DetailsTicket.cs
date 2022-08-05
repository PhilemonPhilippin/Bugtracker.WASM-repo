using Bugtracker.WASM.Models;
using Bugtracker.WASM.Tools;
using Microsoft.AspNetCore.Components;
using static System.Net.WebRequestMethods;
using System.Net.Http.Headers;
using Bugtracker.WASM.Models.MemberModels;
using System.Net.Http.Json;
using Microsoft.JSInterop;

namespace Bugtracker.WASM.Pages.TicketComponents
{
    public partial class DetailsTicket
    {
        [Inject]
        private IMemberLocalStorage LocalStorage { get; set; } = default!;
        [Inject]
        private IApiRequester Requester { get; set; } = default!;
        [Parameter]
        public TicketModel TicketTarget { get; set; }
        [Parameter]
        public EventCallback OnCancel { get; set; }
        private MemberModel MemberAssigned { get; set; } = new MemberModel();
        private MemberModel SubmittingMember { get; set; } = new MemberModel();
        private ProjectModel Project { get; set; } = new ProjectModel();
        private string _token;
        private bool _isMemberConnected;

        protected override async Task OnInitializedAsync()
        {
            _isMemberConnected = await LocalStorage.HasToken();
            if (_isMemberConnected)
            {
                _token = await LocalStorage.GetToken();
                Project = await Requester.Get<ProjectModel>($"Project/{TicketTarget.Project}", _token);

                if (TicketTarget.SubmitMember is not null)
                    SubmittingMember = await Requester.Get<MemberModel>($"Member/{TicketTarget.SubmitMember}", _token);

                if (TicketTarget.AssignedMember is not null)
                    MemberAssigned = await Requester.Get<MemberModel>($"Member/{TicketTarget.AssignedMember}", _token);
            }
        }
    }
}
