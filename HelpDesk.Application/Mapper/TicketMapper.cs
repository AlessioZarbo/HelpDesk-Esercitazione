using HelpDesk.Application.Dto.Ticket;
using HelpDesk.Domain.Entities;
using HelpDesk.Domain.Interfaces;
using HelpDesk.Infrastructure.Interfaces;

namespace HelpDesk.Application.Mapper;

/// <summary>
/// Represents a mapper class for converting between ticket entities and DTOs.
/// </summary>
public class TicketMapper {

	/// <summary>
	/// The provider used for calculating fix hours for tickets.
	/// </summary>
	private readonly IFixHoursProvider _fixHoursProvider;

	/// <summary>
	/// Initializes a new instance of the <see cref="TicketMapper"/> class with the specified fix hours provider.
	/// </summary>
	/// <param name="fixHoursProvider"></param>
	public TicketMapper(IFixHoursProvider fixHoursProvider) {

		_fixHoursProvider = fixHoursProvider;
	}

	/// <summary>
	/// Maps a <see cref="Ticket"/> entity to a <see cref="TicketResponseDto"/>.
	/// </summary>
	/// <param name="entity"></param>
	/// <returns></returns>
	public TicketResponseDto MapToDto(Ticket entity) {

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
	/// Maps a <see cref="Ticket"/> entity to a <see cref="TicketSimpleResponseDto"/>.
	/// </summary>
	/// <param name="entity"></param>
	/// <returns></returns>
	public TicketSimpleResponseDto MapToSimpleDto(Ticket entity) {

		return new TicketSimpleResponseDto() {
			Id = entity.TicketId,
			Title = entity.Title,
			Priority = entity.Priority,
			Status = entity.Status,
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
