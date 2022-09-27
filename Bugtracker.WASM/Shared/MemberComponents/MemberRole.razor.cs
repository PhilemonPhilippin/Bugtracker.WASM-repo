using Bugtracker.WASM.Models.MemberModels;
using Bugtracker.WASM.Tools;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Bugtracker.WASM.Shared.MemberComponents
{
    public partial class MemberRole : ComponentBase
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
        private MemberModel EditedMember { get; set; } = new();
        private MemberModel _myMemberModel = new();
        private int _myMemberId;
        private string _token;

        protected override async Task OnInitializedAsync()
        {
            _token = await LocalStorage.GetToken();
            if (_token is not null)
            {
                _myMemberId = await Requester.Get<int>("Member/idfromjwt", _token);
                _myMemberModel = await Requester.Get<MemberModel>($"Member/{_myMemberId}", _token);
            }
            EditedMember.IdMember = MemberTarget.IdMember;
            EditedMember.Pseudo = MemberTarget.Pseudo;
            EditedMember.Email = MemberTarget.Email;
            EditedMember.Firstname = MemberTarget.Firstname;
            EditedMember.Lastname = MemberTarget.Lastname;
            EditedMember.Role = MemberTarget.Role;
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
            _token = await LocalStorage.GetToken();
            if (_token is not null)
            {
                using HttpResponseMessage response = 
                    await Requester.Put(EditedMember,$"Member/{EditedMember.IdMember}/{EditedMember.Role}", _token);
                if (response.IsSuccessStatusCode)
                {
                    await OnConfirm.InvokeAsync();
                }
            }
        }
    }
}
