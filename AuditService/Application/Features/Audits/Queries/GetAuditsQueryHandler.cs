using System.Collections.Generic;
using System.Linq;
using AuditService.Data;
using MediatR;
using AutoMapper;
using AuditService.Models;
using AuditService.Infrastructure;
using AuditService.Application.Features.Shared;

namespace AuditService.Application.Features.Audits.Queries
{ 
    public class GetAuditsQuery : IRequest<PagedResponse<AuditDto>>
    { 
        public GetAuditsQuery()
        {
            PageNumber = 1;
            PageSize = 100;
        }
        public GetAuditsQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
        public int PageNumber { get; set; }
        public int PageSize { get; set; } 
    }

    public class GetAuditsQueryHandler : RequestHandler<GetAuditsQuery, PagedResponse<AuditDto>>
    {
        private readonly AuditContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IUriService _uriService;

        public GetAuditsQueryHandler(AuditContext dbContext, IMapper mapper, IUriService uriService){
            _dbContext = dbContext;
            _mapper = mapper;
            _uriService = uriService;
        }       

        protected override PagedResponse<AuditDto> Handle(GetAuditsQuery request)
        {
            if (request == null)
            {
                var response = _mapper.ProjectTo<AuditDto>(_dbContext.Audits).OrderBy(a => a.Id).ToList();
                return new PagedResponse<AuditDto>(response);
            }

            var pagination = _mapper.Map<PaginationFilter>(request);
            
            var skip = (request.PageNumber - 1) * request.PageSize;
            var audits = _mapper.ProjectTo<AuditDto>(_dbContext.Audits).OrderBy(a => a.Id)
                .Skip(skip).Take(request.PageSize).ToList();

            var paginationResponse = PaginationHelpers.CreatePaginatedResponse(_uriService, pagination, audits);

            return paginationResponse;
    
        }
    }
}