namespace HelpDesk.Application.Exceptions;

/// <summary>
/// Represents an exception that is thrown when the provided data is not valid, such as when a required field is missing or contains invalid values.
/// </summary>
public class NotValidDataException : Exception {

	/// <summary>
	/// Initializes a new instance of the <see cref="NotValidDataException"/> class with a specified error message.
	/// </summary>
	/// <param name="field"></param>
	public NotValidDataException(string field) : base($"The field '{field}' is not valid.") { }
}
