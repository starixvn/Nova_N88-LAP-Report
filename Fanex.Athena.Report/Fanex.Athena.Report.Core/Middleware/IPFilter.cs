namespace Fanex.Athena.Report.Core.Middleware
{
    using Fanex.Athena.Report.Core.DTO;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;

    public class IPFilter
    {
        private readonly RequestDelegate _next;
        private readonly ApplicationOptions _applicationOptions;

        public IPFilter(RequestDelegate next, IOptions<ApplicationOptions> applicationOptionAccessor)
        {
            _next = next;
            _applicationOptions = applicationOptionAccessor.Value;
        }

        public async Task Invoke(HttpContext context)
        {
            var ipAddress = context.Connection.RemoteIpAddress;
            var whiteListIPList = _applicationOptions.WhiteList ?? new List<string>();

            var isInWhiteListIPList = !whiteListIPList.Any() || whiteListIPList.Any(x => IPAddress.Parse(x).Equals(ipAddress));
            if (!isInWhiteListIPList)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                await context.Response.WriteAsync(JsonConvert.SerializeObject(new
                {
                    msg = $"Ip {ipAddress} was denied ",
                }));
                return;
            }

            await _next.Invoke(context);
        }
    }
}