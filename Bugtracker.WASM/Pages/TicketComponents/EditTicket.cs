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
                using HttpResponseMessage response = await Http.PutAsJsonAsync("https://localhost:7051/api/Ticket", ticketModel);
                if (!response.IsSuccessStatusCode)
                {
                    string message = await response.Content.ReadAsStringAsync();
                    if (message.Contains("Title already exists."))
                        _displayTitleTaken = true;
                }
                else
                {
                    // Vérifier s'il y a une combinaison assign existante
                    if (ticketModel.AssignedMember is not null)
                    {
                        int assignedMemberId = (int)ticketModel.AssignedMember;
                        AssignMinimalModel assign = new AssignMinimalModel() { Project = ticketModel.Project, Member = assignedMemberId };
                        Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
                        using HttpResponseMessage getOneResponse = await Http.PostAsJsonAsync("https://localhost:7051/api/Assign/getone", assign);
                        if (!getOneResponse.IsSuccessStatusCode)
                        {
                            // Dans ce cas, il n'y a pas d'assign. Il faut en créer un
                            // TODO : Eviter de devoir faire deux demandes API pour ça. Une seule devrait suffire
                            await Http.PostAsJsonAsync("https://localhost:7051/api/Assign", assign);
                        }
                    }

                    // Envoyer une requête au ticket controller pour savoir :
                    // Est-ce que l'ancienne combinaison assignedMember/Project est présente dans au moins UN autre ticket ?
                    // Si oui, alors on ne fait rien.
                    // Si non, alors on doit delete cet ancien assign.

                    await OnConfirm.InvokeAsync();
                }
            }
        }
    }
}
