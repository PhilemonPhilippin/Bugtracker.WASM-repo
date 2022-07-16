using Bugtracker.WASM.Models.MemberModels;

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
                PswdHash = memberRegistration.Password
            };
        }
    }
}
