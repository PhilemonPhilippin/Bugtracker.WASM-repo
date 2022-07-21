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
        public HttpClient Http { get; set; }
        [Inject]
        private IMemberLocalStorage LocalStorage { get; set; }
        [Parameter]
        public MemberNoPswdModel MemberTarget { get; set; }
        [Parameter]
        public EventCallback OnCancel { get; set; }
        [Parameter]
        public EventCallback OnConfirm { get; set; }
        private string _token;
        private MemberNoPswdModel MemberEdited { get; set; } = new MemberNoPswdModel();
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
                Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
                using HttpResponseMessage response = await Http.PutAsJsonAsync($"https://localhost:7051/api/Member", MemberEdited);

                if (!response.IsSuccessStatusCode)
                {
                    string errorMessage = await response.Content.ReadAsStringAsync();
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
                else
                    await OnConfirm.InvokeAsync();
            }
        }
    }
}
