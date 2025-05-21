namespace Presentation_Layer.Middleware
{
    public static class AuthenticationStateMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuthenticationStateMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthenticationStateMiddleware>();
        }
    }
}
