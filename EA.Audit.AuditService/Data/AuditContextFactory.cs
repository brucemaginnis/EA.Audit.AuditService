using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace EA.Audit.AuditService.Data
{
    public class AuditContextFactory : IAuditContextFactory
    {
        private readonly HttpContext _httpContext;
        private DbContextOptions _options;

        public AuditContextFactory(IHttpContextAccessor httpContentAccessor,
            DbContextOptions<AuditContext> options)
        {
            _httpContext = httpContentAccessor.HttpContext;
            _options = options;
        }

        public AuditContext AuditContext
        {
            get
            {
                var scopes = _httpContext.User.FindFirst(c => c.Type == "scope").Value.Split(' ');
                if (scopes.Any(s => s == "audit-api/audit_admin")){
                    return new AuditContext(_options, true);
                }
                
                return new AuditContext(_options, TenantId);
            }
        }

        private string TenantId
        {
            get
            {
                ValidateHttpContext();

                var client_id = _httpContext.User.Claims.FirstOrDefault(c => c.Type == "client_id").Value;

                return client_id;
            }
        }

        private void ValidateHttpContext()
        {
            if (this._httpContext == null)
            {
                throw new ArgumentNullException(nameof(this._httpContext));
            }
        }
       
    }
}
