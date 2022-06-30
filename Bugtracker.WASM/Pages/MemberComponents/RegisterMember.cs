using Bugtracker.WASM.ViewModels;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace Bugtracker.WASM.Pages.MemberComponents
{
    public partial class RegisterMember
    {
        [Inject]
        public HttpClient Http { get; set; }

        public MemberRegistrationVm member;
        public RegisterMember()
        {
            member = new MemberRegistrationVm();
        }
        private async Task SubmitRegistration()
        {
            HttpResponseMessage response = await Http.PostAsJsonAsync("https://localhost:7051/api/Member", member);
            // if I ever need to get back the member that I posted :
            // MemberRegistrationVm responseMember = await response.Content.ReadFromJsonAsync<MemberRegistrationVm>();
            member = new MemberRegistrationVm();
        }
    }
}
