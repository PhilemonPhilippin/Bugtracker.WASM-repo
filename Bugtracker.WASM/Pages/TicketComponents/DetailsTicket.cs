using Bugtracker.WASM.Models;
using Bugtracker.WASM.Tools;
using Microsoft.AspNetCore.Components;
using static System.Net.WebRequestMethods;
using System.Net.Http.Headers;
using Bugtracker.WASM.Models.MemberModels;
using System.Net.Http.Json;

namespace Bugtracker.WASM.Pages.TicketComponents
{
    public partial class DetailsTicket
    {
        [Inject]
        private HttpClient Http { get; set; }
        [Inject]
        private IMemberLocalStorage LocalStorage { get; set; }
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
            _token = await LocalStorage.GetToken();
            if (_token is null)
                _isMemberConnected = false;
            else
                _isMemberConnected = true;

            if (_isMemberConnected)
            {
                Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
                Project = await Http.GetFromJsonAsync<ProjectModel>($"https://localhost:7051/api/Project/{TicketTarget.Project}");

                SubmittingMember = await Http.GetFromJsonAsync<MemberModel>($"https://localhost:7051/api/Member/{TicketTarget.SubmitMember}");

                if (TicketTarget.AssignedMember is not null)
                {
                    int memberAssignedId = (int)TicketTarget.AssignedMember;
                    MemberAssigned = await Http.GetFromJsonAsync<MemberModel>($"https://localhost:7051/api/Member/{memberAssignedId}");
                }
            }
        }
    }
}
