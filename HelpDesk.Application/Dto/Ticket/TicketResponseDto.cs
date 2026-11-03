using HelpDesk.Domain.Enums;

namespace HelpDesk.Application.Dto.Ticket;

/// <summary>
/// Represents the data transfer object for creating a new support ticket in the HelpDesk system.
/// </summary>
public class TicketResponseDto {

	/// <summary>
	/// Gets or sets the unique identifier for the ticket.
	/// </summary>
	public int Id { get; set; }

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

	/// <summary>
	/// Gets or sets the status of the ticket.
	/// </summary>
	public required Status Status { get; set; }

	/// <summary>
	/// Gets or sets the user who created the ticket.
	/// </summary>
	public required string CreatedByUserId { get; set; }

	/// <summary>
	/// Gets or sets the user who assigned the ticket.
	/// </summary>
	public string? AssignedToUserId { get; set; }

	/// <summary>
	/// Gets or sets the date and time when the ticket was created.
	/// </summary>
	public DateTime CreatedAt  { get; set; }

	/// <summary>
	/// Gets or sets the date and time when the ticket is due.
	/// </summary>
	public DateTime DueDate { get; set; }
}
