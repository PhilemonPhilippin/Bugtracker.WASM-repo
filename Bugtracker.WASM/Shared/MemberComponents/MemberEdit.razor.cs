﻿using Bugtracker.WASM.Models.MemberModels;
using Bugtracker.WASM.Tools;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Bugtracker.WASM.Shared.MemberComponents
{
    public partial class MemberEdit : ComponentBase
    {
        [Inject]
        private IMemberLocalStorage LocalStorage { get; set; } = default!;
        [Inject]
        private IApiRequester Requester { get; set; } = default!;
        [Inject]
        private IJSRuntime JS { get; set; } = default!;
        [Parameter]
        public MemberModel MemberTarget { get; set; }
        [Parameter]
        public EventCallback OnCancel { get; set; }
        [Parameter]
        public EventCallback OnConfirm { get; set; }
        private ElementReference focusRef;
        private IJSObjectReference? module;
        private string _token;
        private MemberModel MemberEdited { get; set; } = new MemberModel();
        private bool _displayPseudoTaken;
        private bool _displayEmailTaken;
        private bool _isMemberConnected;

        protected override async Task OnInitializedAsync()
        {
            MemberEdited.IdMember = MemberTarget.IdMember;
            MemberEdited.Pseudo = MemberTarget.Pseudo;
            MemberEdited.Email = MemberTarget.Email;
            MemberEdited.Firstname = MemberTarget.Firstname;
            MemberEdited.Lastname = MemberTarget.Lastname;
            MemberEdited.Role = MemberTarget.Role;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                module = await JS.InvokeAsync<IJSObjectReference>("import", "./js/mainscript.js");
                await module.InvokeVoidAsync("ScrollToRef", focusRef);
            }
        }
        private async Task SubmitEdit()
        {
            _displayPseudoTaken = false;
            _displayEmailTaken = false;

            _token = await LocalStorage.GetToken();
            if (_token is not null)
            {
                _isMemberConnected = true;
                using HttpResponseMessage response = await Requester.Put(MemberEdited, "Member", _token);

                if (!response.IsSuccessStatusCode)
                {
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    HandleErrorMessage(errorMessage);
                }
                else
                    await OnConfirm.InvokeAsync();
            }
        }
        private void HandleErrorMessage(string errorMessage)
        {
            if (errorMessage.Contains("Pseudo and Email already exist."))
            {
                _displayPseudoTaken = true;
                _displayEmailTaken = true;
            }
            else if (errorMessage.Contains("Pseudo already exists."))
                _displayPseudoTaken = true;
            else if (errorMessage.Contains("Email already exists."))
                _displayEmailTaken = true;
        }
    }
}
