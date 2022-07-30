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
        private HttpClient Http { get; set; }
        [Inject]
        private IMemberLocalStorage LocalStorage { get; set; }
      
        [Parameter]
        public TicketModel TicketTarget { get; set; }
        [Parameter]
        public EventCallback OnCancel { get; set; }
        private MemberNoPswdModel MemberAssigned { get; set; } = new MemberNoPswdModel();
        private MemberNoPswdModel SubmittingMember { get; set; } = new MemberNoPswdModel();
        private ProjectModel Project { get; set; } = new ProjectModel();
        private string _token;
        private bool _isMemberConnected;
       
        protected override async Task OnInitializedAsync()
        {
            _isMemberConnected = await LocalStorage.HasToken();
            if (_isMemberConnected)
            {
                _token = await LocalStorage.GetToken();
                Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
                Project = await Http.GetFromJsonAsync<ProjectModel>($"https://localhost:7051/api/Project/{TicketTarget.Project}");

                if (TicketTarget.SubmitMember is not null)
                {
                    SubmittingMember = await Http.GetFromJsonAsync<MemberNoPswdModel>($"https://localhost:7051/api/Member/{TicketTarget.SubmitMember}");
                }

                if (TicketTarget.AssignedMember is not null)
                {
                    MemberAssigned = await Http.GetFromJsonAsync<MemberNoPswdModel>($"https://localhost:7051/api/Member/{TicketTarget.AssignedMember}");
                }
            }
        }
    }
}
