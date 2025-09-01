using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.WebApi.Common;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using System.Text.Json;

namespace Ambev.DeveloperEvaluation.WebApi.Middleware
{
    public class ValidationExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ValidationExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (DomainException ex)
            {
                var response = new ApiResponse
                {
                    Success = false,
                    Message = ex.Message,
                    Errors = Enumerable.Empty<ValidationErrorDetail>()
                };
                await HandleExceptionAsync(context, ex, StatusCodes.Status400BadRequest, response);
            }
            catch (Ambev.DeveloperEvaluation.Application.Exceptions.ApplicationException ex)
            {
                var response = new ApiResponse
                {
                    Success = false,
                    Message = ex.Message,
                    Errors = Enumerable.Empty<ValidationErrorDetail>()
                };
                await HandleExceptionAsync(context, ex, StatusCodes.Status400BadRequest, response);
            }
            catch (InvalidOperationException ex)
            {
                var response = new ApiResponse
                {
                    Success = false,
                    Message = ex.Message,
                    Errors = Enumerable.Empty<ValidationErrorDetail>()
                };
                await HandleExceptionAsync(context, ex, StatusCodes.Status400BadRequest, response);
            }
            catch (ArgumentException ex)
            {
                var response = new ApiResponse
                {
                    Success = false,
                    Message = ex.Message,
                    Errors = Enumerable.Empty<ValidationErrorDetail>()
                };
                await HandleExceptionAsync(context, ex, StatusCodes.Status400BadRequest, response);
            }
            catch (UnauthorizedAccessException ex)
            {
                var response = new ApiResponse
                {
                    Success = false,
                    Message = ex.Message,
                    Errors = Enumerable.Empty<ValidationErrorDetail>()
                };
                await HandleExceptionAsync(context, ex, StatusCodes.Status400BadRequest, response);
            }
            catch (ValidationException ex)
            {
                var response = new ApiResponse
                {
                    Success = false,
                    Message = "Validation Failed",
                    Errors = ex.Errors
                        .Select(error => (ValidationErrorDetail)error)
                };
                await HandleExceptionAsync(context, ex, StatusCodes.Status400BadRequest, response);
            }
            catch (KeyNotFoundException ex)
            {
                var response = new ApiResponse
                {
                    Success = false,
                    Message = ex.Message,
                    Errors = Enumerable.Empty<ValidationErrorDetail>()
                };
                await HandleExceptionAsync(context, ex, StatusCodes.Status404NotFound, response);
            }
            catch (Exception ex)
            {
                var response = new ApiResponse
                {
                    Success = false,
                    Message = ex.Message,
                    Errors = Enumerable.Empty<ValidationErrorDetail>()
                };
                await HandleExceptionAsync(context, ex, StatusCodes.Status500InternalServerError, response);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception, int statusCode, ApiResponse response)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            Console.WriteLine($"Handler Exception: {exception.Message}");
            return context.Response.WriteAsync(JsonSerializer.Serialize(response, jsonOptions));
        }
    }
}
