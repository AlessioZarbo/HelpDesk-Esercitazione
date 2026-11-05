using System.Text.Json;
using FluentValidation;
using HelpDesk.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.API.Middlewares;

/// <summary>
/// Middleware for handling exceptions in the application.
/// </summary>
public class ExceptionHandlingMiddleware {

	/// <summary>
	/// The next middleware in the pipeline.
	/// </summary>
	private readonly RequestDelegate _next;

	/// <summary>
	/// The logger for logging exception details.
	/// </summary>
	private readonly ILogger<ExceptionHandlingMiddleware> _logger;

	/// <summary>
	/// Initializes a new instance of the <see cref="ExceptionHandlingMiddleware"/> class.
	/// </summary>
	/// <param name="next"></param>
	/// <param name="logger"></param>
	public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger) {

		_next = next;
		_logger = logger;
	}

	/// <summary>
	/// Invokes the middleware to handle exceptions during the request processing.
	/// </summary>
	/// <param name="context"></param>
	public async Task InvokeAsync(HttpContext context) {

		try {

			await _next(context);
		}
		catch (NotImplementedException e) {
			_logger.LogWarning(e, "Endpoint is not implemented");
			await WriteProblemDetailsAsync(
				context,
				StatusCodes.Status501NotImplemented,
				"Endpoint not implemented",
				e.Message);
		}
		catch (NotValidDataException ex) {
			_logger.LogWarning(ex, "Invalid request data");
			await WriteProblemDetailsAsync(
				context,
				StatusCodes.Status400BadRequest,
				"Not valid Data",
				ex.Message);
		}
		catch (BadRequestException ex) {
			_logger.LogWarning(ex, "Bad request");
			await WriteProblemDetailsAsync(
				context,
				StatusCodes.Status400BadRequest,
				"Bad request",
				ex.Message);
		}
		catch (InvalidDataException ex) {
			_logger.LogWarning(ex, "Invalid request data");
			await WriteProblemDetailsAsync(
				context,
				StatusCodes.Status400BadRequest,
				"Bad request",
				ex.Message);
		}
		catch (NotSupportedException ex) {
			_logger.LogWarning(ex, "Unsupported request format");
			await WriteProblemDetailsAsync(
				context,
				StatusCodes.Status400BadRequest,
				"Unsupported format",
				ex.Message);
		}
		catch (NotFoundException ex) {
			_logger.LogWarning(ex, "Resource not found");
			await WriteProblemDetailsAsync(
				context,
				StatusCodes.Status404NotFound,
				"Resource not found",
				ex.Message);
		}
		catch (ConflictException ex) {
			_logger.LogWarning(ex, "Conflict");
			await WriteProblemDetailsAsync(
				context,
				StatusCodes.Status409Conflict,
				"Conflict",
				ex.Message);
		}
		catch (ValidationException ex) {
			_logger.LogWarning(ex, "Validation failed");
			await WriteProblemDetailsAsync(
				context,
				StatusCodes.Status400BadRequest,
				"Validation error",
				ex.Message);
		}
		catch (UnauthorizedAccessException ex) {
			_logger.LogWarning(ex, "Unauthorized request");
			await WriteProblemDetailsAsync(
				context,
				StatusCodes.Status401Unauthorized,
				"Access denied",
				ex.Message);
		}
		catch (DbUpdateConcurrencyException ex) {
			_logger.LogWarning(ex, "Database concurrency conflict");
			await WriteProblemDetailsAsync(
				context,
				StatusCodes.Status409Conflict,
				"Concurrency conflict",
				ex.Message);
		}
		catch (DbUpdateException ex) {
			_logger.LogError(ex, "Database update error");
			await WriteProblemDetailsAsync(
				context,
				StatusCodes.Status500InternalServerError,
				"Database update error",
				ex.Message);
		}
		catch (Exception e) {
			_logger.LogError(e, "Unhandled exception");
			await WriteProblemDetailsAsync(
				context,
				StatusCodes.Status500InternalServerError,
				"An unexpected error occurred");
		}
	}

	/// <summary>
	/// Writes problem details to the response.
	/// </summary>
	/// <param name="context"></param>
	/// <param name="statusCode"></param>
	/// <param name="title"></param>
	/// <param name="detail"></param>
	/// <returns></returns>
	public async Task WriteProblemDetailsAsync(
		HttpContext context,
		int statusCode,
		string title,
		string? detail = null) {

		context.Response.StatusCode = statusCode;
		context.Response.ContentType = "application/problem+json";

		var problemDetails = new ProblemDetails {
			Status = statusCode,
			Title = title,
			Detail = detail
		};

		string json = JsonSerializer.Serialize(problemDetails);
		await context.Response.WriteAsync(json);
	}
}
