namespace Presentation_Layer.Middleware
{
    public class AuthenticationStateMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthenticationStateMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // If user is not authenticated and not on login page, redirect to login
            // Check if the user is not authenticated
            bool isAuthenticated = context.User.Identity?.IsAuthenticated ?? false;
            if (!isAuthenticated)
            {
                var path = context.Request.Path.Value?.ToLower();
                if (path != null &&
                    !path.Equals("/") &&
                    !path.Equals("/account/forgetpassword")&&
                    !path.Equals("/account/resetpassword") &&
                    !path.Equals("/account/checkyourinbox") &&
                    !path.StartsWith("/account/login") &&
                    !path.StartsWith("/account/register") &&
                    !path.StartsWith("/home/welcome") &&
                    !path.StartsWith("/home/index") &&
                    !path.StartsWith("/css/") &&
                    !path.StartsWith("/js/") &&
                    !path.StartsWith("/lib/") &&
                    !path.StartsWith("/images/"))
                {
                    context.Response.Redirect("/Account/Login");
                    return;
                }
            }

            await _next(context);
        }
    }

}