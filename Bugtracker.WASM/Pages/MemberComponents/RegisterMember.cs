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
            var responseObject = await response.Content.ReadFromJsonAsync<object?>();
            bool isRegistrationCompleted = false;
            if (responseObject is not null)
            {
                switch (responseObject)
                {
                    case int i:
                        switch (i)
                        {
                            case -123:
                                isLoginTaken = true;
                                // TODO : deal with the state of isLoginTaken to show to the user that login is taken
                                Console.WriteLine("Login taken");
                                break;
                            case -456:
                                Console.WriteLine("Email taken");
                                isEmailTaken = true;
                                break;
                            case -789:
                                isLoginTaken = true;
                                isEmailTaken = true;
                                Console.WriteLine("Both Login and Email taken");
                                break;
                            default:
                                Console.WriteLine("Response object not as expected");
                                break;
                        }
                        break;
                    case MemberRegistrationVm responseMember:
                        isRegistrationCompleted = true;
                        break;
                    default:
                        Console.WriteLine("Response object not as expected");
                        break;
                }
            }
            if (isRegistrationCompleted)
                member = new MemberRegistrationVm();
        }
    }
}
