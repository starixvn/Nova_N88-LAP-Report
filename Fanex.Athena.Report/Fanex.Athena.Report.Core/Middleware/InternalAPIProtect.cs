namespace Fanex.Athena.Report.Core.Middleware
{
    using Fanex.Athena.Report.Core.DTO;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Options;
    using System.Net;
    using System.Threading.Tasks;

    public class InternalAPIProtect
    {
        private readonly RequestDelegate _next;
        private readonly ApplicationOptions _applicationOptions;
        private const string HeaderAuthKey = "X-Internal-Auth-Key";

        public InternalAPIProtect(RequestDelegate next, IOptions<ApplicationOptions> applicationOptionAccessor)
        {
            _next = next;
            _applicationOptions = applicationOptionAccessor.Value;
        }

        public async Task Invoke(HttpContext context)
        {
            var isIgnored = context.Request.Path.Value.Contains("favicon.ico");
            if (!isIgnored)
            {
                var postedKey = context.Request.Headers[HeaderAuthKey];
                var isAuthorized = !string.IsNullOrEmpty(postedKey) && postedKey.Equals(_applicationOptions.AuthKey);
                if (!isAuthorized)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    await context.Response.WriteAsync("Auth token was invalid");
                    return;
                }
            }
            await _next.Invoke(context);
        }
    }
}