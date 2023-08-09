namespace BigDataAcademy.Api.Middlewares;

public class SwaggerRedirectMiddleware
{
    private readonly RequestDelegate next;

    public SwaggerRedirectMiddleware(RequestDelegate next)
        => this.next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path.Value == "/")
        {
            context.Response.Redirect("/swagger/index.html");
            return;
        }

        await this.next(context);
    }
}
