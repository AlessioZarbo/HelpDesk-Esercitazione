using HelpDesk.Domain.Enums;

namespace HelpDesk.Application.Dto.Ticket;

/// <summary>
/// Represents the data transfer object for creating a new support ticket in the HelpDesk system.
/// </summary>
public class TicketRequestDto {

	/// <summary>
	/// Gets or sets the title of the ticket.
	/// </summary>
	public required string Title { get; set; }

	/// <summary>
	/// Gets or sets the description of the ticket.
	/// </summary>
	public required string Description { get; set; }

	/// <summary>
	/// Gets or sets the priority level of the ticket.
	/// </summary>
	public required Priority Priority { get; set; }
}
