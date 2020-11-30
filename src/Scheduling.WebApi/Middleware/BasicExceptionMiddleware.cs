using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Scheduling.WebApi.ApiModels;
using System;
using System.Threading.Tasks;

namespace Scheduling.WebApi.Middleware
{
    public class BasicExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public BasicExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext ctx)
        {
            try
            {
                await _next.Invoke(ctx);
            }
            catch (Exception ex)
            {
                var res = JsonConvert.SerializeObject(new ExceptionResponse
                {
                    Message = ex.Message,
                    StackTrace = ex.StackTrace
                });
                ctx.Response.ContentType = "application/json";
                ctx.Response.StatusCode = 500;
                await ctx.Response.WriteAsync(res);
            }
        }
    }
}
