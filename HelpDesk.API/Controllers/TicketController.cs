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
	/// Creates a new ticket.
	/// </summary>
	/// <param name="ticketRequestDto"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	[HttpPost]
	[Authorize(Roles = "User")]
	[ProducesResponseType(StatusCodes.Status201Created)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> CreateAsync([FromBody] TicketRequestDto ticketRequestDto, CancellationToken cancellationToken) {

		var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
		var ticket = await _ticketService.CreateAsync(ticketRequestDto, userId, cancellationToken);
		return CreatedAtAction(nameof(GetByTicketIdAsync), new { id = ticket.Id }, ticket);
	}

	/// <summary>
	/// Gets all tickets for the authenticated user or operator.
	/// </summary>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	[HttpGet]
	[Authorize(Roles = "User,Operator")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken) {

		var isOperator = User.IsInRole("Operator");
		var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
		return Ok(await _ticketService.GetAllAsync(isOperator, userId, cancellationToken));
	}

	/// <summary>
	/// Gets a ticket by its ID.
	/// </summary>
	/// <param name="id"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	[HttpGet("{id:int}")]
	[Authorize(Roles = "User,Operator")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ActionName(nameof(GetByTicketIdAsync))]
	public async Task<IActionResult> GetByTicketIdAsync([FromRoute] int id, CancellationToken cancellationToken) {

		var isOperator = User.IsInRole("Operator");
		var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
		return Ok(await _ticketService.GetByTicketIdAsync(id, isOperator, userId, cancellationToken));
	}

	/// <summary>
	/// Updates the status of a ticket.
	/// </summary>
	/// <param name="id"></param>
	/// <param name="ticketStatusUpdateRequestDto"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	[HttpPatch("{id:int}/status")]
	[Authorize(Roles = "Operator")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status409Conflict)]
	public async Task<IActionResult> UpdateTicketStatusAsync([FromRoute] int id, [FromBody] TicketStatusUpdateRequestDto ticketStatusUpdateRequestDto, CancellationToken cancellationToken) {

		var ticket = await _ticketService.SetStatusAsync(id, ticketStatusUpdateRequestDto, cancellationToken);
		return Ok(ticket);
	}
}
