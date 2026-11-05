namespace HelpDesk.Application.Exceptions;

/// <summary>
/// Represents an exception that is thrown when a bad request is made, such as when the request data is invalid or missing required information.
/// </summary>
public class BadRequestException : Exception {

	/// <summary>
	/// Initializes a new instance of the <see cref="BadRequestException"/> class with a specified error message.
	/// </summary>
	/// <param name="message"></param>
	public BadRequestException(string message) : base(message) { }

}
