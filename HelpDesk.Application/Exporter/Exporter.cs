using System.Text.Json;
using HelpDesk.Domain.Entities;

namespace HelpDesk.Application.Exporter;

/// <summary>
/// Represents an interface for exporting ticket data in different formats.
/// </summary>
public interface ITicketExporter {

	/// <summary>
	/// Exports the specified ticket to a string representation in a specific format.
	/// </summary>
	/// <param name="tickets"></param>
	/// <returns></returns>
	string Export(List<Ticket> tickets);
}

/// <summary>
/// Represents a CSV exporter for ticket data.
/// </summary>
public sealed class CsvTicketExporter : ITicketExporter {

	/// <summary>
	/// Exports the specified ticket to a CSV string representation.
	/// </summary>
	/// <param name="tickets"></param>
	/// <returns></returns>
	public string Export(List<Ticket> tickets) {

		const string header = "ticketId,title,priority,status,createdAt,dueDate";

		var rows = tickets.Select(ticket => string.Join(",",
			ticket.TicketId,
			Escape(ticket.Title),
			ticket.Status,
			ticket.Priority,
			ticket.DueDate.ToString("o")));

		return string.Join("\n", new[] { header }.Concat(rows));
	}

	/// <summary>
	/// Escapes a string value for CSV formatting by enclosing it in double quotes and replacing any existing double quotes with two double quotes.
	/// </summary>
	/// <param name="value"></param>
	/// <returns></returns>
	private static string Escape(string value) =>
		"\"" + value.Replace("\"", "\"\"") + "\"";
}

/// <summary>
/// Represents a JSON exporter for ticket data.
/// </summary>
public sealed class JsonTicketExporter : ITicketExporter {

	/// <summary>
	/// Exports the specified ticket to a JSON string representation.
	/// </summary>
	/// <param name="tickets"></param>
	/// <returns></returns>
	public string Export(List<Ticket> tickets) =>
		JsonSerializer.Serialize(tickets.Select(ticket => new
		{
			ticketId = ticket.TicketId,
			title = ticket.Title,
			status = ticket.Status.ToString(),
			priority = ticket.Priority.ToString(),
			dueDate = ticket.DueDate
		}));
}

/// <summary>
/// Represents a factory class for creating ticket exporters based on the specified format.
/// </summary>
public static class TicketExporterFactory {

	/// <summary>
	/// Creates a ticket exporter based on the specified format.
	/// </summary>
	/// <param name="format"></param>
	/// <returns></returns>
	/// <exception cref="NotSupportedException"></exception>
	public static ITicketExporter Create(string format) =>
		format.Trim().ToLowerInvariant() switch
		{
			"csv" => new CsvTicketExporter(),
			"json" => new JsonTicketExporter(),
			_ => throw new NotSupportedException($"Formato '{format}' non supportato.")
		};
}
