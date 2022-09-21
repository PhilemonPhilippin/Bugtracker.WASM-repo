using Bugtracker.WASM.Models.MemberModels;


namespace Bugtracker.WASM.Mappers
{
    internal static class MemberMapper
    {
        public static MemberPostModel ToPostModel(this MemberRegistrationModel memberRegistration)
        {
            return new MemberPostModel()
            {
                Pseudo = memberRegistration.Pseudo,
                Email = memberRegistration.Email,
                Password = memberRegistration.Password
            };
        }
    }
}
