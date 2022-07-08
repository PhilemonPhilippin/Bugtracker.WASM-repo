using Bugtracker.WASM.Models;
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
        public int MemberEditId { get; set; }
        [Parameter]
        public EventCallback OnCancel { get; set; }
        [Parameter]
        public EventCallback OnConfirm { get; set; }
        private string _token;
        private MemberEditModel MemberEdited { get; set; } = new MemberEditModel();
        private bool _displayPseudoTaken;
        private bool _displayEmailTaken;
        private bool _isMemberConnected;

        protected override async Task OnInitializedAsync()
        {
            await AskTokenValidation();
            Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            HttpResponseMessage response = await Http.GetAsync($"https://localhost:7051/api/Member/{MemberEditId}");

            if (response.IsSuccessStatusCode)
            {
                MemberModel memberModel = await response.Content.ReadFromJsonAsync<MemberModel>();
                MemberEdited.IdMember = memberModel.IdMember;
                MemberEdited.Pseudo = memberModel.Pseudo;
                MemberEdited.Email = memberModel.Email;
                MemberEdited.Firstname = memberModel.Firstname;
                MemberEdited.Lastname = memberModel.Lastname;
            }
        }
        private async Task SubmitEdit()
        {
            _displayPseudoTaken = false;
            _displayEmailTaken = false;

            await AskTokenValidation();
            Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            HttpResponseMessage response = await Http.PutAsJsonAsync($"https://localhost:7051/api/Member/{MemberEditId}", MemberEdited);

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
        private async Task AskTokenValidation()
        {
            _token = await LocalStorage.GetToken();
            if (_token is null)
                _isMemberConnected = false;
            else
            {
                Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
                HttpResponseMessage response = await Http.GetAsync("https://localhost:7051/api/Member/token");
                if (!response.IsSuccessStatusCode)
                    _isMemberConnected = false;
                else
                {
                    ConnectedMemberModel connectedMember = await response.Content.ReadFromJsonAsync<ConnectedMemberModel>();
                    await LocalStorage.SetToken(connectedMember.Token);
                    _isMemberConnected = true;
                }
            }
        }
    }
}
