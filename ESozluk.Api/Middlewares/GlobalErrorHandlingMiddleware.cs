using ESozluk.Core.Exceptions;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;

namespace ESozluk.Api.Middlewares
{
    public class GlobalErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }


        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var statusCode = (int)HttpStatusCode.InternalServerError;
            string message = "Sunucu hatası";

            if (exception is NotFoundException)
            {
                statusCode = (int)HttpStatusCode.NotFound;
                message = exception.Message;
            }
            else if (exception is ValidationException)
            {
                statusCode = (int)HttpStatusCode.BadRequest;
                message = exception.Message;
            }
            // YENİ EKLENEN KISIM: YETKİLENDİRME HATASI
            else if (exception is AuthorizedAccessException)
            {
                statusCode = (int)HttpStatusCode.Forbidden; // Veya 401
                message = "Bu işlemi yapmaya yetkiniz yok.";
            }

            context.Response.StatusCode = statusCode; // Response kodunu da set edelim

            
            return context.Response.WriteAsync(message);
        }


    }
}