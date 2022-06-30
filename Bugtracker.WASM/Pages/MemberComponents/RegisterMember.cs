using Bugtracker.WASM.ViewModels;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace Bugtracker.WASM.Pages.MemberComponents
{
    public partial class RegisterMember
    {
        [Inject]
        public HttpClient Http { get; set; }
        public bool isLoginTaken = false;
        public bool isEmailTaken = false;
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
            //object? responseObject = await response.Content.ReadFromJsonAsync<object?>();
            if (!response.IsSuccessStatusCode)
            {
                int responseNumber = await response.Content.ReadFromJsonAsync<int>();
                switch (responseNumber)
                {
                    case -123:
                        // TODO : deal with the state of isLoginTaken to show to the user that login is taken
                        isLoginTaken = true;
                        break;
                    case -456:
                        isEmailTaken = true;
                        break;
                    case -789:
                        isLoginTaken = true;
                        isEmailTaken = true;
                        break;
                    default:
                        Console.WriteLine("Response number not as expected");
                        break;
                }
            }
            else
                navManager.NavigateTo("dashboard");
        }
    }
}
