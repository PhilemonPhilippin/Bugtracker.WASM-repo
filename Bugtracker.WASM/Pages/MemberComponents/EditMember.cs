using Bugtracker.WASM.Models.MemberModels;
using Bugtracker.WASM.Tools;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Bugtracker.WASM.Pages.MemberComponents
{
    public partial class EditMember
    {
        [Inject]
        private IMemberLocalStorage LocalStorage { get; set; } = default!;
        [Inject]
        private IApiRequester Requester { get; set; } = default!;
        [Parameter]
        public MemberModel MemberTarget { get; set; }
        [Parameter]
        public EventCallback OnCancel { get; set; }
        [Parameter]
        public EventCallback OnConfirm { get; set; }
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
        }
        private async Task SubmitEdit()
        {
            _displayPseudoTaken = false;
            _displayEmailTaken = false;

            _isMemberConnected = await LocalStorage.HasToken();
            if (_isMemberConnected)
            {
                _token = await LocalStorage.GetToken();
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
