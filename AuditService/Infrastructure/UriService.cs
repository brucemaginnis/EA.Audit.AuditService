using System;
using AuditService.Application.Queries;
using AuditService.Controllers;
using Microsoft.AspNetCore.WebUtilities;

namespace AuditService.Infrastructure
{
    public class UriService : IUriService
    {
        private readonly string _baseUri;
        
        public UriService(string baseUri)
        {
            _baseUri = baseUri;
        }
        
        public Uri GetAuditUri(string postId)
        {
            return new Uri(_baseUri + ApiRoutes.Audits.Get.Replace("{Id}", postId));
        }

        public Uri GetAllAuditsUri(PaginationQuery pagination = null)
        {
            var uri = new Uri(_baseUri);

            if (pagination == null)
            {
                return uri;
            }

            var modifiedUri = QueryHelpers.AddQueryString(_baseUri + ApiRoutes.Audits.GetAll, "pageNumber", pagination.PageNumber.ToString());
            modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "pageSize", pagination.PageSize.ToString());
            
            return new Uri(modifiedUri);
        }
    }
}