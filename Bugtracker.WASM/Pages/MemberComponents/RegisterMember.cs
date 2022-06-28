using Bugtracker.WASM.ViewModels;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace Bugtracker.WASM.Pages.MemberComponents
{
    public partial class RegisterMember
    {
        [Inject]
        public HttpClient Http { get; set; }

        private MemberRegistrationVm _member;

        protected MemberRegistrationVm Member
        {
            get { return _member; }
            set { _member = value; }
        }
        public RegisterMember()
        {
            _member = new MemberRegistrationVm();
        }
        private async Task SubmitRegistration()
        {
            // TODO : mettre à jour l'uri du post
            using var response = await Http.PutAsJsonAsync("ChangeMyURIHere", _member);
            MemberRegistrationVm responseMember = await response.Content.ReadFromJsonAsync<MemberRegistrationVm>();
        }
    }
}
