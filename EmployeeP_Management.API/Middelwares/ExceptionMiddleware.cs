using System.Net;

namespace EmployeeP_Management.API.Middelwares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.ContentType = "application/json";

                context.Response.StatusCode = ex switch
                {
                    KeyNotFoundException => (int)HttpStatusCode.NotFound,
                    InvalidOperationException => (int)HttpStatusCode.BadRequest,
                    _ => (int)HttpStatusCode.InternalServerError
                };

                await context.Response.WriteAsJsonAsync(new
                {
                    StatusCode = context.Response.StatusCode,
                    Message = ex.Message
                });
            }
        }

    }
}
