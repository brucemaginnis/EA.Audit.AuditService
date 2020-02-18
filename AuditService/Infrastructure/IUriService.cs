using System;
using AuditService.Application.Queries;

namespace AuditService.Infrastructure
{
    public interface IUriService
    {
        Uri GetAuditUri(string postId);
        Uri GetAllAuditsUri(PaginationQuery pagination = null);
    }
}