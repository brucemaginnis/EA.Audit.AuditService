using AuditService.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;

namespace AuditService.Data
{
    public class AuditContextFacotry : IAuditContextFactory
    {
        private readonly HttpContext _httpContext;
        private DbContextOptions<AuditContext> _options;

        public AuditContextFacotry(IHttpContextAccessor httpContentAccessor,
            DbContextOptions<AuditContext> options)
        {
            _httpContext = httpContentAccessor.HttpContext;
            _options = options;
        }

        public AuditContext AuditContext => new AuditContext(_options, TenantId);

        private Guid TenantId
        {
            get
            {
                ValidateHttpContext();

                var tenantId = this._httpContext.Request.Headers[Constants.Tenant.TenantId].ToString();

                return MarshallTenantId(tenantId);
            }
        }

        private void ValidateHttpContext()
        {
            if (this._httpContext == null)
            {
                throw new ArgumentNullException(nameof(this._httpContext));
            }
        }

        private static Guid MarshallTenantId(string tenantId)
        {
            if (tenantId == null)
            {
                throw new ArgumentNullException(nameof(tenantId));
            }

            if (!Guid.TryParse(tenantId, out Guid tenantGuid))
            {
                throw new ArgumentNullException(nameof(tenantId));
            }

            return tenantGuid;
        }
    }
}
