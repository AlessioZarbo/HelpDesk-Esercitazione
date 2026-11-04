using HelpDesk.Domain.Enums;

namespace HelpDesk.Application.Dto.Ticket;

/// <summary>
/// Represents the data transfer object for updating the status of a support ticket in the HelpDesk system.
/// </summary>
public class TicketStatusUpdateRequestDto {

	/// <summary>
	/// Gets or sets the new status of the ticket.
	/// </summary>
	public required Status Status { get; set; }
}
