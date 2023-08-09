using System.Net;

namespace BigDataAcademy.Api.Middlewares;

public class IntermittentFailureMiddleware
{
    private readonly RequestDelegate next;

    public IntermittentFailureMiddleware(RequestDelegate next)
        => this.next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        var rnd = new Random();

        if (rnd.Next(0, 20) == 0 && context.Request.Path.Value!.Contains("claim-zone"))
        {
            context.Response.StatusCode = (int)HttpStatusCode.ServiceUnavailable;
            return;
        }

        await this.next(context);
    }
}
