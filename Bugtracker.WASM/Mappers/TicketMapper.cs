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
        public static TicketFormModel ToFormModel(this TicketModel ticketModel)
        {
            return new TicketFormModel()
            {
                IdTicket = ticketModel.IdTicket,
                Title = ticketModel.Title,
                Status = ticketModel.Status,
                Priority = ticketModel.Priority,
                Type = ticketModel.Type,
                Description = ticketModel.Description,
                SubmitTime = ticketModel.SubmitTime,
                SubmitMember = ticketModel.SubmitMember,
                AssignedMember = ticketModel.AssignedMember,
                Project = ticketModel.Project,
            };
        }
    }
}
