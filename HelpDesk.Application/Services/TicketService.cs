using HelpDesk.Application.Dto.Ticket;
using HelpDesk.Application.Mapper;
using HelpDesk.Domain.Entities;
using HelpDesk.Domain.Enums;
using HelpDesk.Domain.Interfaces;
using HelpDesk.Infrastructure.Interfaces;

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
	/// Initializes a new instance of the <see cref="TicketService"/> class with the specified ticket repository.
	/// </summary>
	/// <param name="ticketRepository"></param>
	/// <param name="mapper"></param>
	public TicketService(ITicketRepository ticketRepository, TicketMapper mapper) {

		_ticketRepository = ticketRepository;
		_mapper = mapper;
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
			throw new ArgumentException("User ID must be provided for non-operator users.");

		var tickets = isOperator
			? await _ticketRepository.GetAllAsync(cancellationToken)
			: await _ticketRepository.GetAllByUserIdAsync(userId!, cancellationToken);

		return tickets.Select(t => _mapper.MapToSimpleDto(t)).ToList();
	}

	public Task<TicketSimpleResponseDto> GetByTicketIdAsync(int id) {
		throw new NotImplementedException();
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
		return _mapper.MapToDto(ticketResponse);
	}

	public Task SetStatusAsync(string ticketId, Status status) {
		throw new NotImplementedException();
	}
}
