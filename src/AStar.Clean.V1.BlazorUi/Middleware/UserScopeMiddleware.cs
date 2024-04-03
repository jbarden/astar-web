using System.Text.RegularExpressions;

namespace AStar.Clean.V1.BlazorUI.Middleware;

/// <summary>
/// The <see cref="UserScopeMiddleware" /> creates a logging scope that will be used throughout the request.
/// </summary>
public class UserScopeMiddleware
{
    private readonly RequestDelegate next;
    private readonly ILogger<UserScopeMiddleware> logger;

    public UserScopeMiddleware(RequestDelegate next, ILogger<UserScopeMiddleware> logger)
    {
        this.next = next;
        this.logger = logger;
    }

    /// <summary>
    /// The InvokeAsync method is called during the ASP.Net Core Lifecycle pipeline - hence VS shows "0 references".
    /// <para>As part of the scope, the Username (masked) and Sub (SubjectId) are automatically included in the log data.</para>
    /// </summary>
    /// <param name="context">
    /// This is the context applicable to the request being processed.
    /// </param>
    /// <returns>
    /// </returns>
    public async Task InvokeAsync(HttpContext context)
    {
        if(context.User.Identity is { IsAuthenticated: true })
        {
            var user = context.User;
            const string pattern = @"(?<=[\w]{1})[\w-\._\+%]*(?=[\w]{1}@)";
            var maskedUsername = Regex.Replace(user.Identity?.Name ?? "", pattern, m => new('*', m.Length));

            var subjectId = user.Claims.First(c => c.Type == "sub")?.Value;

            using(logger.BeginScope("User:{user}, SubjectId:{subject}", maskedUsername, subjectId))
            {
                await next(context);
            }
        }
        else
        {
            await next(context);
        }
    }
}
