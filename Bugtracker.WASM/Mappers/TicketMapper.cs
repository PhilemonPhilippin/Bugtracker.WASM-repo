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
                AssignedMember = ticketModel.AssignedMember,
                Project = ticketModel.Project,
            };
        }
        public static TicketEditModel ToEditModel(this TicketModel ticketModel)
        {
            return new TicketEditModel()
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
        public static TicketModel ToModel(this TicketEditModel editModel)
        {
            return new TicketModel()
            {
                IdTicket = editModel.IdTicket,
                Title = editModel.Title,
                Status = editModel.Status,
                Priority = editModel.Priority,
                Type = editModel.Type,
                Description = editModel.Description,
                SubmitTime = editModel.SubmitTime,
                SubmitMember = editModel.SubmitMember,
                AssignedMember = editModel.AssignedMember,
                Project = editModel.Project,
            };
        }
    }
}
