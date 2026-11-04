using FluentValidation;
using HelpDesk.Application.Dto.Ticket;

namespace HelpDesk.Application.Validators;

/// <summary>
/// Validator for the TicketRequestDto class, which validates the properties of a ticket creation request.
/// </summary>
public class TicketRequestDtoValidator : AbstractValidator<TicketRequestDto> {

	/// <summary>
	/// Initializes a new instance of the TicketCreateDtoValidator class and defines the validation rules for the TicketRequestDto properties.
	/// </summary>
	public TicketRequestDtoValidator() {

		// Validation rules for the TicketRequestDto properties
		RuleFor(t => t.Priority)
			.Cascade(CascadeMode.Stop)
			.IsInEnum();

		// Validation rules for the Title and Description properties
		RuleFor(t => t.Title.Length)
			.GreaterThan(4)
			.LessThan(101);

		// Validation rules for the Description property
		RuleFor(t => t.Description.Length)
			.LessThan(2001);
	}
}
