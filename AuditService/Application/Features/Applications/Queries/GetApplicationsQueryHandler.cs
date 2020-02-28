using AuditService.Application.Extensions;
using AuditService.Application.Features.Shared;
using AuditService.Data;
using AuditService.Infrastructure;
using AuditService.Models;
using AutoMapper;
using MediatR;
using System.Linq;

namespace AuditService.Application.Features.Application.Queries
{
    public class GetAuditApplicationsQuery : IRequest<PagedResponse<ApplicationDto>>
    {
        public GetAuditApplicationsQuery()
        {
            PageNumber = 1;
            PageSize = 100;
        }
        public GetAuditApplicationsQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

    public class GetApplicationsQueryHandler : RequestHandler<GetAuditApplicationsQuery, PagedResponse<ApplicationDto>>
    {
        private readonly AuditContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IUriService _uriService;

        public GetApplicationsQueryHandler(IAuditContextFactory dbContextFactory, IMapper mapper, IUriService uriService)
        {
            _dbContext = dbContextFactory.AuditContext;
            _mapper = mapper;
            _uriService = uriService;
        }

        protected override PagedResponse<ApplicationDto> Handle(GetAuditApplicationsQuery request)
        {
            if (request == null)
            {
                var response = _mapper.ProjectTo<ApplicationDto>(_dbContext.AuditApplications).OrderBy(a => a.Id).ToList();
                return new PagedResponse<ApplicationDto>(response);
            }

            var pagination = _mapper.Map<PaginationFilter>(request);

            var skip = (request.PageNumber) * request.PageSize;
            var query = _mapper.ProjectTo<ApplicationDto>(_dbContext.AuditApplications).OrderBy(a => a.Id)
                .Skip(skip).Take(request.PageSize).ToSql();

            var audits = _mapper.ProjectTo<ApplicationDto>(_dbContext.AuditApplications).OrderBy(a => a.Id)
                .Skip(skip).Take(request.PageSize).ToList();

            var paginationResponse = PaginationHelpers.CreatePaginatedResponse(_uriService, pagination, audits);

            return paginationResponse;

        }
    }
}
