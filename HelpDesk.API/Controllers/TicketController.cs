using HelpDesk.Application.Dto.Ticket;
using HelpDesk.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HelpDesk.API.Controllers;

/// <summary>
/// Represents the controller responsible for handling ticket-related API requests in the HelpDesk system.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class TicketController : ControllerBase {

	/// <summary>
	/// The ticket service used for handling ticket-related operations.
	/// </summary>
	private readonly ITicketService _ticketService;

	/// <summary>
	/// Initializes a new instance of the <see cref="TicketController"/> class with the specified ticket service.
	/// </summary>
	/// <param name="ticketService"></param>
	public TicketController(ITicketService ticketService) {

		_ticketService = ticketService;
	}

	[HttpGet]
	public Task<IEnumerable<TicketResponseDto>> GetAllAsync() {

		return _ticketService.GetAllAsync();
	}
}
