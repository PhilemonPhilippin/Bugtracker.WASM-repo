using Bugtracker.WASM.Models;

namespace Bugtracker.WASM.Mappers
{
    internal static class TicketMapper
    {
        public static TicketModel ToModel(this TicketFormModel formModel)
        {
            return new TicketModel()
            {
                IdTicket = (formModel.IdTicket == 0) ? 0 : formModel.IdTicket,
                Title = formModel.Title,
                Status = formModel.Status,
                Priority = formModel.Priority,
                Type = formModel.Type,
                Description = formModel.Description,
                SubmitTime = formModel.SubmitTime,
                SubmitMember = formModel.SubmitMember,
                AssignedMember = formModel.AssignedMember,
                Project = formModel.Project,
            };
        }
    }
}
