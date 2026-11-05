using HelpDesk.Application.Dto.Ticket;
using HelpDesk.Application.Exceptions;
using HelpDesk.Application.Exporter;
using HelpDesk.Application.Mapper;
using HelpDesk.Domain.Entities;
using HelpDesk.Domain.Enums;
using HelpDesk.Domain.Interfaces;
using HelpDesk.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;

namespace HelpDesk.Application.Services;

public class TicketService : ITicketService {

	/// <summary>
	/// The repository used for accessing ticket data.
	/// </summary>
	private readonly ITicketRepository _ticketRepository;

	/// <summary>
	/// The mapper used for converting between ticket entities and DTOs.
	/// </summary>
	private readonly TicketMapper _mapper;

	/// <summary>
	/// The logger used for logging information and errors related to ticket operations.
	/// </summary>
	private readonly ILogger<TicketService> _logger;

	/// <summary>
	/// Initializes a new instance of the <see cref="TicketService"/> class with the specified ticket repository.
	/// </summary>
	/// <param name="ticketRepository"></param>
	/// <param name="mapper"></param>
	/// <param name="logger"></param>
	public TicketService(ITicketRepository ticketRepository, TicketMapper mapper, ILogger<TicketService> logger) {

		_ticketRepository = ticketRepository;
		_mapper = mapper;
		_logger = logger;
	}

	/// <summary>
	/// Gets all tickets asynchronously.
	/// </summary>
	/// <param name="isOperator"></param>
	/// <param name="userId"></param>
	/// <param name="cancellationToken"></param>
	/// <returns>A task that represents the asynchronous operation. The task result contains the list of all tickets.</returns>
	public async Task<List<TicketSimpleResponseDto>> GetAllAsync(bool isOperator, string? userId, CancellationToken cancellationToken) {

		if (!isOperator && string.IsNullOrEmpty(userId))
			throw new InvalidDataException("User ID must be provided for non-operator users.");

		var tickets = isOperator
			? await _ticketRepository.GetAllAsync(cancellationToken)
			: await _ticketRepository.GetAllAsync(userId!, cancellationToken);

		return tickets.Select(t => _mapper.MapToSimpleDto(t)).ToList();
	}

	/// <summary>
	/// Gets a ticket by its ID asynchronously.
	/// </summary>
	/// <param name="ticketId"></param>
	/// <param name="isOperator"></param>
	/// <param name="userId"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	/// <exception cref="ArgumentException"></exception>
	public async Task<TicketResponseDto> GetByTicketIdAsync(int ticketId, bool isOperator, string? userId, CancellationToken cancellationToken) {

		if (ticketId <= 0)
			throw new InvalidDataException("Ticket ID must be greater than zero.");

		if (!isOperator && string.IsNullOrEmpty(userId))
			throw new InvalidDataException("User ID must be provided for non-operator users.");

		var ticket = isOperator
			?  await _ticketRepository.GetByTicketIdAsync(ticketId, cancellationToken)
			: await _ticketRepository.GetByTicketIdAsync(ticketId, userId!, cancellationToken);

		return ticket == null ? throw new NotFoundException(nameof(ticket), ticketId) : _mapper.MapToDto(ticket);
	}

	/// <summary>
	/// Creates a new ticket asynchronously.
	/// </summary>
	/// <param name="ticket"></param>
	/// <param name="userId"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public async Task<TicketResponseDto> CreateAsync(TicketRequestDto ticket, string? userId, CancellationToken cancellationToken) {

		var ticketResponse = await _ticketRepository.CreateAsync(_mapper.MapToEntity(ticket, userId), cancellationToken);

		_logger.LogInformation(
			"Ticket created {TicketId} by {UserId}",
			ticketResponse.TicketId,
			userId);
		return _mapper.MapToDto(ticketResponse);
	}

	/// <summary>
	/// Sets the status of a ticket asynchronously.
	/// </summary>
	/// <param name="operatorId"></param>
	/// <param name="ticketId"></param>
	/// <param name="ticketStatusUpdateRequestDto"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public async Task<TicketResponseDto> SetStatusAsync(string? operatorId, int ticketId, TicketStatusUpdateRequestDto ticketStatusUpdateRequestDto, CancellationToken cancellationToken) {

		if (ticketId <= 0)
			throw new InvalidDataException("Ticket ID must be greater than zero.");

		var ticket = await _ticketRepository.GetByTicketIdAsync(ticketId, cancellationToken);
		if (ticket == null)
			throw new NotFoundException(nameof(ticket), ticketId);

		CheckStatusAvailability(ticket.Status, ticketStatusUpdateRequestDto.Status);

		if (ticketStatusUpdateRequestDto.Status == Status.InProgress)
		{
			if (string.IsNullOrEmpty(operatorId))
				throw new InvalidDataException("Operator ID must be provided.");

			ticket.AssignedToUserId ??= operatorId;
		}

		ticket.Status = ticketStatusUpdateRequestDto.Status;
		await _ticketRepository.SetStatusAsync(ticket, cancellationToken);

		_logger.LogInformation(
			"Ticket status updated {TicketId} to {Status} by {OperatorId}",
			ticketId,
			ticket.Status,
			operatorId);
		return _mapper.MapToDto(ticket);
	}

	/// <summary>
	/// Exports tickets in the specified format asynchronously.
	/// </summary>
	/// <param name="format"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public async Task<string> ExportAsync(string format, CancellationToken cancellationToken) {

		var exporter = TicketExporterFactory.Create(format);
		var tickets = await _ticketRepository.GetAllAsync(cancellationToken);
		var content = exporter.Export(tickets);

		_logger.LogInformation("Tickets exported in {Format} format", format);
		return content;
	}

	/// <summary>
	/// Checks the availability of a status update.
	/// </summary>
	/// <param name="ticketStatus"></param>
	/// <param name="dtoStatus"></param>
	public static void CheckStatusAvailability(Status ticketStatus, Status dtoStatus) {

		var isValidTransition = (ticketStatus, dtoStatus) switch
		{
			(Status.Open, Status.InProgress) => true,
			(Status.InProgress, Status.Resolved) => true,
			(Status.Resolved, Status.Closed) => true,
			_ => false
		};

		if (!isValidTransition)
			throw new ConflictException(
				$"Invalid status transition from {ticketStatus} to {dtoStatus}.");
	}
}
