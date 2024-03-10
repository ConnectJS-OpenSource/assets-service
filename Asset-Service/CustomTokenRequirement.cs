using Microsoft.AspNetCore.Authorization;

namespace Asset_Service;

public class CustomTokenRequirement(IConfiguration configuration) : IAuthorizationRequirement
{
    public string AuthToken => configuration.GetValue<string>("AuthToken") ?? Guid.NewGuid().ToString();
}

public class CustomTokenRequirementHandler(IHttpContextAccessor httpContextAccessor)
    : AuthorizationHandler<CustomTokenRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, CustomTokenRequirement requirement)
    {
        var ctx = httpContextAccessor.HttpContext;
        if (ctx != null)
        {
            var authHeader = ctx.Request.Headers.Authorization.FirstOrDefault();
            authHeader = authHeader?.Replace("bearer ", "").Replace("Bearer ", "").Replace("BEARER ", "");

            var keys = requirement.AuthToken;
            if (authHeader != null && keys == authHeader)
                context.Succeed(requirement);
            else
            {
                ctx.Response.StatusCode = 401;
                await ctx.Response.WriteAsJsonAsync(new
                {
                    message = "Token Error"
                });
                await ctx.Response.CompleteAsync();
                context.Fail();
            }
        }
        else
            context.Fail();
    }
}