using FluentValidation;
using HelpDesk.Application.Dto.Ticket;

namespace HelpDesk.Application.Validators;

/// <summary>
/// Validator for the TicketStatusUpdateRequestDto class, which validates the properties of a ticket status update request.
/// </summary>
public class TicketStatusUpdateValidator : AbstractValidator<TicketStatusUpdateRequestDto> {

	/// <summary>
	/// Initializes a new instance of the TicketStatusUpdateValidator class and defines the validation rules for the TicketStatusUpdateRequestDto properties.
	/// </summary>
	public TicketStatusUpdateValidator() {

		RuleFor(t => t.Status)
			.Cascade(CascadeMode.Stop)
			.IsInEnum();
	}
}
