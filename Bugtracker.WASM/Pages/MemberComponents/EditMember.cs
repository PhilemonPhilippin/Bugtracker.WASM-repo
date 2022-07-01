using Bugtracker.WASM.Models;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace Bugtracker.WASM.Pages.MemberComponents
{
    public partial class EditMember
    {
        [Inject]
        public HttpClient Http { get; set; }
        // Faut-il initialiser le MemberTarget?
        // Ou faut-il faire une fonction async on initialize pour le MemberEditModel ?
        [Parameter]
        public MemberModel MemberTarget { get; set; }

        public MemberEditModel memberEdit;
        public bool isLoginTaken = false;
        public bool isEmailTaken = false;
        public EditMember()
        {
            MemberTarget = new MemberModel();
            memberEdit = new MemberEditModel()
            {
                IdMember = MemberTarget.IdMember,
                Login = MemberTarget.Login,
                Password = MemberTarget.Password,
                EmailAddress = MemberTarget.EmailAddress,
                Firstname = MemberTarget.Firstname,
                Lastname = MemberTarget.Lastname
            };
        }

        private async Task SubmitEdit(int id)
        {
            isLoginTaken = false;
            isEmailTaken = false;
            // TODO : finir cette fonction et le formulaire qui va avec
            HttpResponseMessage response = await Http.PutAsJsonAsync($"https://localhost:7051/api/Member/{id}", memberEdit);

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
                Console.WriteLine("Close the Edit member component");
        }
    }
}
