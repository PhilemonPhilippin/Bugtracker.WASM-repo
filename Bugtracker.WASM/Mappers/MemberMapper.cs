using Bugtracker.WASM.Models;

namespace Bugtracker.WASM.Mappers
{
    internal static class MemberMapper
    {

        public static MemberModel ToModel(this MemberRegistrationModel memberRegistration)
        {
            return new MemberModel()
            {
                IdMember = 0,
                Pseudo = memberRegistration.Pseudo,
                Email = memberRegistration.Email,
                PswdHash = memberRegistration.Password,
                Firstname = memberRegistration.Firstname,
                Lastname = memberRegistration.Lastname
            };
        }
    }
}
