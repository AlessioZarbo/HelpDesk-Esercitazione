namespace HelpDesk.Domain.Enums;

/// <summary>
/// Represents the status of support tickets in the HelpDesk system.
/// </summary>
public enum Status {

	/// <summary>
	/// Indicates that the ticket is currently open and awaiting action.
	/// </summary>
	Open = 1,

	/// <summary>
	/// Indicates that the ticket is currently being worked on or addressed.
	/// </summary>
	InProgress = 2,

	/// <summary>
	/// Indicates that the ticket has been resolved and is awaiting closure or confirmation from the user.
	/// </summary>
	Resolved = 3,

	/// <summary>
	/// Indicates that the ticket has been closed and no further action is required.
	/// </summary>
	Closed = 4,
}
