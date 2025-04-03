namespace dispatch_order_api.middlewares
{
    public class CustomAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        const string UNAUTHORIZED_ERROR = "Unauthorized request..!";
        const string SERVER_ERROR = "Unable to authenticate. Contact system administrator.!";

        public CustomAuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/swagger"))
            {
                // Skip API key check for Swagger endpoints
                await _next(context);
                return;
            }

            if (!context.Request.Headers.TryGetValue("X-API-KEY", out var apiKey))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync(UNAUTHORIZED_ERROR);
                return;
            }

            var validApiKey = Environment.GetEnvironmentVariable("API_KEY");

            if (string.IsNullOrEmpty(validApiKey))
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsync(SERVER_ERROR);
                return;
            }

            if (apiKey != validApiKey)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync(UNAUTHORIZED_ERROR);
                return;
            }

            await _next(context);
        }
    }

}
