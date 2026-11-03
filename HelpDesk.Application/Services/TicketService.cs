using HelpDesk.Application.Dto.Ticket;
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
	/// The provider used for calculating fix hours for tickets.
	/// </summary>
	private readonly IFixHoursProvider _fixHoursProvider;

	/// <summary>
	/// Initializes a new instance of the <see cref="TicketService"/> class with the specified ticket repository.
	/// </summary>
	/// <param name="ticketRepository"></param>
	/// <param name="fixHoursProvider"></param>
	public TicketService(ITicketRepository ticketRepository, IFixHoursProvider fixHoursProvider) {

		_fixHoursProvider = fixHoursProvider;
		_ticketRepository = ticketRepository;
	}

	/// <summary>
	/// Gets all tickets asynchronously.
	/// </summary>
	/// <returns>A task that represents the asynchronous operation. The task result contains the list of all tickets.</returns>
	public Task<IEnumerable<TicketResponseDto>> GetAllAsync() {

		return _ticketRepository.GetAllAsync().ContinueWith(t => t.Result.Select(MapToDto));
	}

	public Task<TicketResponseDto> GetByIdAsync(int id) {
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

		var ticketResponse = await _ticketRepository.CreateAsync(MapToEntity(ticket, userId), cancellationToken);
		return MapToDto(ticketResponse);
	}

	public Task SetStatusAsync(string ticketId, Status status) {
		throw new NotImplementedException();
	}

	/// <summary>
	/// Maps a <see cref="Ticket"/> entity to a <see cref="TicketResponseDto"/>.
	/// </summary>
	/// <param name="entity"></param>
	/// <returns></returns>
	public static TicketResponseDto MapToDto(Ticket entity) {

		return new TicketResponseDto() {
			Id = entity.TicketId,
			Title = entity.Title,
			Description = entity.Description,
			Priority = entity.Priority,
			Status = entity.Status,
			CreatedByUserId = entity.CreatedByUserId,
			AssignedToUserId = entity.AssignedToUserId,
			CreatedAt = entity.CreatedAt,
			DueDate = entity.DueDate,
		};
	}

	/// <summary>
	/// Maps a <see cref="TicketRequestDto"/> to a <see cref="Ticket"/> entity.
	/// </summary>
	/// <param name="dto"></param>
	/// <param name="userId"></param>
	/// <returns></returns>
	public Ticket MapToEntity(TicketRequestDto dto, string? userId) {

		var ticket = Ticket.Create(
			title: dto.Title,
			description: dto.Description,
			priority: dto.Priority,
			userId: userId,
			fixHours: _fixHoursProvider // Accedendo ai dati di istanza non può essere statico
		);
		return ticket;
	}
}
