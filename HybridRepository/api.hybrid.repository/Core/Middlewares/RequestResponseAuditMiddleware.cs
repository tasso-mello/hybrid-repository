namespace api.hybrid.repository.Core.Middlewares
{
    using core.hybrid.repository.Utilities;
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Http;
    using System;
    using System.Threading.Tasks;

    public class RequestResponseAuditMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestResponseAuditMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if(!string.IsNullOrEmpty(context.Request.Headers["Authorization"].ToString()) && MethodIsNotAllowAnonymous(context.GetEndpoint()))
                Extensions._userId = new Guid(context.Request.Headers["Authorization"].ToString().ReadAuthorizationToken("identifier"));

            await _next(context);
        }

        private bool MethodIsNotAllowAnonymous(Endpoint endpoint)
            => !(endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() is object);
    }
}
