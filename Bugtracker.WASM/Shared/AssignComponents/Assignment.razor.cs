using Bugtracker.WASM.Models.MemberModels;
using Bugtracker.WASM.Models;
using Bugtracker.WASM.Tools;
using Microsoft.AspNetCore.Components;
using Bugtracker.WASM.Mappers;
using Microsoft.JSInterop;

namespace Bugtracker.WASM.Shared.AssignComponents
{
    public partial class Assignment
    {
        [Inject]
        private IMemberLocalStorage LocalStorage { get; set; } = default!;
        [Inject]
        private IApiRequester Requester { get; set; } = default!;
        [Inject]
        private IJSRuntime JS { get; set; } = default!;
        [Parameter]
        public TicketModel TicketTarget { get; set; }
        [Parameter]
        public EventCallback OnCancel { get; set; }
        [Parameter]
        public EventCallback OnConfirm { get; set; }
        private List<MemberModel> _members = new List<MemberModel>();
        private List<TicketModel> _tickets = new List<TicketModel>();
        private TicketEditModel EditedTicket { get; set; } = new TicketEditModel();
        private ElementReference focusRef;
        private IJSObjectReference? module;
        private string _token;
        private bool _isMemberConnected;

        protected override async Task OnInitializedAsync()
        {
            EditedTicket = TicketTarget.ToEditModel();
            _token = await LocalStorage.GetToken();
            if (_token is not null)
            {
                _isMemberConnected = true;
                _members = await Requester.Get<List<MemberModel>>("Member", _token);
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

        private async Task SubmitAssign()
        {
            _token = await LocalStorage.GetToken();
            if (_token is not null)
            {
                _isMemberConnected = true;
                TicketModel ticketModel = EditedTicket.ToModel();

                //I Get my list of tickets before editing the ticket so that i can use it later to check for old existing assign.
                _tickets = await Requester.Get<List<TicketModel>>("Ticket", _token);

                using HttpResponseMessage responseEdit = await Requester.Put(ticketModel, "Ticket", _token);
                if (responseEdit.IsSuccessStatusCode)
                {
                    // SI pas de assign existant entre ce membre et projet
                    if (_tickets.Count(t => t.AssignedMember == ticketModel.AssignedMember && t.Project == ticketModel.Project) == 0)
                    {
                        // ALORS créer un nouvel assign
                        int newAssignMemberId = (int)ticketModel.AssignedMember;
                        AssignMinimalModel newAssign = new AssignMinimalModel() { Project = ticketModel.Project, Member = newAssignMemberId };
                        await Requester.Post(newAssign, "Assign", _token);
                    }

                    // (TicketTarget est l'entité qui a les valeurs de mon ticket avant l'edit)
                    // SI il y avait déjà un AssignedMember avant l'edit
                    if (TicketTarget.AssignedMember is not null)
                    {
                        // ET SI ce ticket (avant l'edit) est le seul avec cette combinaison Project <=> AssignedMember
                        if (_tickets.Count(t => t.AssignedMember == TicketTarget.AssignedMember && t.Project == TicketTarget.Project) == 1)
                        {
                            int oldAssignedMember = (int)TicketTarget.AssignedMember;
                            await Requester.Delete($"Assign/{TicketTarget.Project}/{oldAssignedMember}", _token);
                        }
                    }
                    await OnConfirm.InvokeAsync();
                }
            }
        }
    }
}
