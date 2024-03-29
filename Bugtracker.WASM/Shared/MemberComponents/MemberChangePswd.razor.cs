﻿using Bugtracker.WASM.Models.MemberModels;
using Bugtracker.WASM.Tools;
using Microsoft.AspNetCore.Components;

namespace Bugtracker.WASM.Shared.MemberComponents
{
    public partial class MemberChangePswd : ComponentBase
    {
        [Inject]
        private IMemberLocalStorage LocalStorage { get; set; } = default!;
        [Inject]
        private IApiRequester Requester { get; set; } = default!;
        [Parameter]
        public int MemberId { get; set; }
        [Parameter]
        public EventCallback OnCancel { get; set; }
        private MemberFormPswdModel pswdModel = new MemberFormPswdModel();
        private MemberPostPswdModel postPswdModel = new MemberPostPswdModel();
        private bool _displayIncorrectPassword;
        private bool _displayPasswordChanged;
        private bool _isMemberConnected;
        private string _token;

        private async Task Submit()
        {
            _displayIncorrectPassword = false;
            _displayPasswordChanged = false;

            _token = await LocalStorage.GetToken();
            if (_token is not null)
            {
                _isMemberConnected = true;
                postPswdModel.IdMember = MemberId;
                postPswdModel.NewPassword = pswdModel.NewPassword;
                postPswdModel.OldPassword = pswdModel.OldPassword;

                using HttpResponseMessage response = await Requester.Post(postPswdModel, "Member/changepswd", _token);
                if (!response.IsSuccessStatusCode)
                {
                    string errorMessage = await response.Content.ReadAsStringAsync();

                    if (errorMessage.Contains("Password incorrect."))
                        _displayIncorrectPassword = true;
                }
                else
                {
                    _displayPasswordChanged = true;
                    pswdModel = new MemberFormPswdModel();
                }
            }
        }
    }
}
