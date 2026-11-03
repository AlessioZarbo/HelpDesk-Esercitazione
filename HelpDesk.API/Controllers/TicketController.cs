using System.Security.Claims;
using HelpDesk.Application.Dto.Ticket;
using HelpDesk.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelpDesk.API.Controllers;

/// <summary>
/// Represents the controller responsible for handling ticket-related API requests in the HelpDesk system.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
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

	/// <summary>
	/// Gets all tickets.
	/// </summary>
	/// <returns>The list of all tickets.</returns>
	[HttpGet]
	public async Task<IEnumerable<TicketResponseDto>> GetAllAsync() {

		return await _ticketService.GetAllAsync();
	}

	/// <summary>
	/// Creates a new ticket.
	/// </summary>
	/// <param name="ticketRequestDto"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	[HttpPost]
	[Authorize(Roles = "User")]
	[ProducesResponseType(StatusCodes.Status201Created)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> CreateAsync(TicketRequestDto ticketRequestDto, CancellationToken cancellationToken) {

		var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
		var ticket = await _ticketService.CreateAsync(ticketRequestDto, userId, cancellationToken);
		return StatusCode(201, ticket);
	}
}
