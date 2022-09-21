﻿using Bugtracker.WASM.Models.MemberModels;
using Bugtracker.WASM.Models;
using Bugtracker.WASM.Tools;
using Microsoft.AspNetCore.Components;

namespace Bugtracker.WASM.Shared.TicketComponents
{
    public partial class TicketDetail
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
            _token = await LocalStorage.GetToken();
            if (_token is not null)
            {
                _isMemberConnected = true;
                Project = await Requester.Get<ProjectModel>($"Project/{TicketTarget.Project}", _token);

                SubmittingMember = await Requester.Get<MemberModel>($"Member/{TicketTarget.SubmitMember}", _token);

                if (TicketTarget.AssignedMember is not null)
                    MemberAssigned = await Requester.Get<MemberModel>($"Member/{TicketTarget.AssignedMember}", _token);
            }
        }
    }
}