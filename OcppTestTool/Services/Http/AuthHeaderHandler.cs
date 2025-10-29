using OcppTestTool.Services.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OcppTestTool.Services.Http
{
    public sealed class AuthHeaderHandler : DelegatingHandler
    {
        private readonly ISessionService _session;

        public AuthHeaderHandler(ISessionService session) => _session = session;

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var user = _session.CurrentUser;
            // 토큰 사용 시:
            if (!string.IsNullOrEmpty(user?.Token)) 
                request.Headers.Authorization = new("Bearer", user.Token);

            return base.SendAsync(request, cancellationToken);
        }
    }
}
