namespace api.hybrid.repository.Core.Middlewares
{
    using FluentValidation;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class ErrorHandlingMiddleware : IMiddleware
	{
		private readonly ILogger<ErrorHandlingMiddleware> _logger;

		public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger)
		{
			_logger = logger;
		}

		public async Task InvokeAsync(HttpContext context, RequestDelegate next)
		{
			int statusCode = 200;
			try
			{
				await next(context);
			}
			catch (Exception ex)
			{
				_logger.LogError($"Unexpected error: {ex}");
				await HandleExceptionAsync(context, ex, statusCode);
			}
		}

		private static Task HandleExceptionAsync(HttpContext context, Exception exception, int statusCode = 400)
		{
			//Defaults
			context.Response.ContentType = "application/json";
			context.Response.StatusCode = statusCode;

			//Log the error
			List<string> errors = new List<string>();

			//Exception types and JSON Responses
			if (exception is ArgumentException) context.Response.StatusCode = statusCode;
			if (exception is ValidationException)
			{
				context.Response.StatusCode = 400;

				foreach (var error in ((ValidationException)exception).Errors)
				{
					errors.Add(error.ErrorMessage);
				}
				return context.Response.WriteAsync(JsonConvert.SerializeObject(errors));
			}

			var json = new
			{
				context.Response.StatusCode,
				exception.Message
			};
			context.Response.StatusCode = statusCode;

			return context.Response.WriteAsync(JsonConvert.SerializeObject(json));
		}
	}
}
