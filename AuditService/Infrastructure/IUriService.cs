using AuditService.Application.Features.Shared;
using System;

namespace AuditService.Infrastructure
{
    public interface IUriService
    {
        Uri GetAuditUri(string postId);
        Uri GetAllAuditsUri(PaginationQuery pagination = null);
    }
}