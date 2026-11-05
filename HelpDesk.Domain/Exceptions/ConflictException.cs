namespace HelpDesk.Application.Exceptions;

/// <summary>
/// Represents an exception that is thrown when a conflict occurs, such as when trying to create a resource that already exists.
/// </summary>
public class ConflictException : Exception{

	/// <summary>
	/// Initializes a new instance of the <see cref="ConflictException"/> class with a specified error message.
	/// </summary>
	/// <param name="message"></param>
	public ConflictException(string message) : base(message) { }
}
