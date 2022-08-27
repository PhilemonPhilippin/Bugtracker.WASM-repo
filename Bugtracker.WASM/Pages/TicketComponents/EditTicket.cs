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
        //private List<MemberModel> _members = new List<MemberModel>();
        private List<TicketModel> _tickets = new List<TicketModel>();
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
                //_members = await Requester.Get<List<MemberModel>>("Member", _token);
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
                // I Get my list of tickets before editing the ticket so that i can use it later to check for old existing assign.
                //_tickets = await Requester.Get<List<TicketModel>>("Ticket", _token);

                using HttpResponseMessage response = await Requester.Put(ticketModel, "Ticket", _token);
                if (!response.IsSuccessStatusCode)
                {
                    string message = await response.Content.ReadAsStringAsync();
                    if (message.Contains("Title already exists."))
                        _displayTitleTaken = true;
                }
                else
                {
                    //if (ticketModel.AssignedMember is not null)
                    //{
                    //    // On ajoute un Assign avec la nouvelle combinaison AssignedMember + Project si il n'y en a pas déjà un.
                    //    int assignedMemberId = (int)ticketModel.AssignedMember;
                    //    AssignMinimalModel newAssign = new AssignMinimalModel() { Project = ticketModel.Project, Member = assignedMemberId };
                    //    await Requester.Post(newAssign, "Assign", _token);
                    //}
                    //if (TicketTarget.AssignedMember is not null)
                    //{
                    //    // On supprime l'Assign de l'ancienne combinaison AssignedMember + Project si aucun autre ticket n'a cette combinaison.
                    //    int assignedMemberId = (int)TicketTarget.AssignedMember;
                    //    AssignMinimalModel oldAssign = new AssignMinimalModel() { Project = TicketTarget.Project, Member = assignedMemberId };
                    //    // Si parmi tous les tickets il n'y en a qu'un avec l'ancienne combinaison, on le supprime.
                    //    if (_tickets.Count(ticket => ticket.AssignedMember == TicketTarget.AssignedMember && ticket.Project == TicketTarget.Project) == 1)
                    //    {
                    //        await Requester.Delete($"Assign/{oldAssign.Project}/{oldAssign.Member}", _token);
                    //    }
                    //}
                    await OnConfirm.InvokeAsync();
                }
            }
        }
    }
}
