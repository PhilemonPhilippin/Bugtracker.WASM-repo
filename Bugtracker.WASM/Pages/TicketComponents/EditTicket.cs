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
        private HttpClient Http { get; set; }
        [Inject]
        private IMemberLocalStorage LocalStorage { get; set; }
        [Parameter]
        public EventCallback OnCancel { get; set; }
        [Parameter]
        public EventCallback OnConfirm { get; set; }
        [Parameter]
        public TicketModel TicketTarget { get; set; }
        private List<ProjectModel> _projects = new List<ProjectModel>();
        private List<MemberModel> _members = new List<MemberModel>();
        private List<TicketModel> _tickets = new List<TicketModel>();
        private TicketEditModel EditedTicket { get; set; } = new TicketEditModel();
        private string _token;
        private bool _isMemberConnected;
        private bool _displayTitleTaken;

        protected override async Task OnInitializedAsync()
        {
            EditedTicket = TicketTarget.ToEditModel();

            _isMemberConnected = await LocalStorage.HasToken();
            if (_isMemberConnected)
            {
                _token = await LocalStorage.GetToken();
                Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
                _projects = await Http.GetFromJsonAsync<List<ProjectModel>>("https://localhost:7051/api/Project");
                _members = await Http.GetFromJsonAsync<List<MemberModel>>("https://localhost:7051/api/Member");
            }
        }
        private async Task SubmitEdit()
        {
            _isMemberConnected = await LocalStorage.HasToken();
            if (_isMemberConnected)
            {
                _displayTitleTaken = false;
                TicketModel ticketModel = EditedTicket.ToModel();
                _token = await LocalStorage.GetToken();
                Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
                // I Get my list of tickets before editing the ticket so that i can use it later to check for old existing assign.
                _tickets = await Http.GetFromJsonAsync<List<TicketModel>>("https://localhost:7051/api/Ticket");
                using HttpResponseMessage response = await Http.PutAsJsonAsync("https://localhost:7051/api/Ticket", ticketModel);
                if (!response.IsSuccessStatusCode)
                {
                    string message = await response.Content.ReadAsStringAsync();
                    if (message.Contains("Title already exists."))
                        _displayTitleTaken = true;
                }
                else
                {
                    if (ticketModel.AssignedMember is not null)
                    {
                        // On ajoute un Assign avec la nouvelle combinaison AssignedMember + Project si il n'y en a pas déjà un.
                        int assignedMemberId = (int)ticketModel.AssignedMember;
                        AssignMinimalModel newAssign = new AssignMinimalModel() { Project = ticketModel.Project, Member = assignedMemberId };
                        await Http.PostAsJsonAsync("https://localhost:7051/api/Assign", newAssign);
                    }
                    if (TicketTarget.AssignedMember is not null)
                    {
                        // On supprime l'Assign de l'ancienne combinaison AssignedMember + Project si aucun autre ticket n'a cette combinaison.
                        int assignedMemberId = (int)TicketTarget.AssignedMember;
                        AssignMinimalModel oldAssign = new AssignMinimalModel() { Project = TicketTarget.Project, Member = assignedMemberId };
                        // Si parmi tous les tickets il n'y en a qu'un avec l'ancienne combinaison, on le supprime.
                        if (_tickets.Count(ticket => ticket.AssignedMember == TicketTarget.AssignedMember && ticket.Project == TicketTarget.Project) == 1)
                        {
                            await Http.DeleteAsync($"https://localhost:7051/api/Assign/{oldAssign.Project}/{oldAssign.Member}");
                        }
                    }
                    await OnConfirm.InvokeAsync();
                }
            }
        }
    }
}
