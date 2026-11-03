using HelpDesk.Domain.Enums;

namespace HelpDesk.Application.Dto.Ticket;

/// <summary>
/// Represents a simplified response DTO for a ticket, containing essential information about the ticket.
/// </summary>
public class TicketSimpleResponseDto {

	/// <summary>
	/// Gets or sets the unique identifier of the ticket.
	/// </summary>
	public int  Id { get; set; }

	/// <summary>
	/// Gets or sets the title of the ticket.
	/// </summary>
	public required string Title { get; set; }

	/// <summary>
	/// Gets or sets the priority of the ticket, represented by the <see cref="Priority"/> enum.
	/// </summary>
	public required Priority Priority { get; set; }

	/// <summary>
	/// Gets or sets the status of the ticket, represented by the <see cref="Status"/> enum.
	/// </summary>
	public required Status Status { get; set; }

	/// <summary>
	/// Gets or sets the due date of the ticket.
	/// </summary>
	public required DateTime DueDate { get; set; }
}
