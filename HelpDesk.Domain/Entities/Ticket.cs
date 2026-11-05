using HelpDesk.Application.Exceptions;
using HelpDesk.Domain.Enums;
using HelpDesk.Domain.Interfaces;

namespace HelpDesk.Domain.Entities;

/// <summary>
/// Represents a support ticket in the HelpDesk system.
/// </summary>
public class Ticket {

	/// <summary>
	/// Gets or sets the unique identifier for the ticket.
	/// </summary>
	public int TicketId { get; set; }

	/// <summary>
	/// Gets or sets the title of the ticket.
	/// </summary>
	public required string Title { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the description of the ticket.
	/// </summary>
	public required string Description { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the priority level of the ticket.
	/// </summary>
	public required Priority Priority { get; set; }

	/// <summary>
	/// Gets or sets the status of the ticket.
	/// </summary>
	public required Status Status { get; set; }

	/// <summary>
	/// Gets or sets the ID of the user who created the ticket.
	/// </summary>
	public required string CreatedByUserId { get; set; }

	/// <summary>
	/// Gets or sets the ID of the user to whom the ticket is assigned.
	/// </summary>
	public string? AssignedToUserId { get; set; }

	/// <summary>
	/// Gets or sets the date and time when the ticket was created.
	/// </summary>
	public DateTime CreatedAt { get; set; }

	/// <summary>
	/// Gets or sets the date and time when the ticket was last updated.
	/// </summary>
	public DateTime DueDate { get; set; }

	/// <summary>
	/// Creates a new instance of the <see cref="Ticket"/> class with the specified properties.
	/// </summary>
	/// <param name="title"></param>
	/// <param name="description"></param>
	/// <param name="priority"></param>
	/// <param name="userId"></param>
	/// <param name="fixHours"></param>
	/// <returns></returns>
	/// <exception cref="ArgumentException"></exception>
	public static Ticket Create(
		string title,
		string description,
		Priority priority,
		string? userId,
		IFixHoursProvider fixHours)
	{
		if (string.IsNullOrWhiteSpace(title))
			throw new NotValidDataException(nameof(title));

		if (string.IsNullOrWhiteSpace(description))
			throw new NotValidDataException(nameof(description));

		if (string.IsNullOrWhiteSpace(userId))
			throw new NotValidDataException(nameof(userId));


		// Calculate the due date based on the priority and fix hours
		var fixHour = fixHours.GetHours(priority);
		var now = DateTime.UtcNow;

		return new Ticket {
			Title = title,
			Description = description,
			Priority = priority,
			CreatedByUserId = userId,
			AssignedToUserId = null,
			Status = Enums.Status.Open,
			CreatedAt = now,
			DueDate = now.AddHours(fixHour)
		};
	}
}
