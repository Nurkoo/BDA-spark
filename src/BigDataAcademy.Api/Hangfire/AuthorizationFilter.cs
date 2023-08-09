using Hangfire.Dashboard;

namespace BigDataAcademy.Api.Hangfire;

public class AuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        return true;
    }
}
