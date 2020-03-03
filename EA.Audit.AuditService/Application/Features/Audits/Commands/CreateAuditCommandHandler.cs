using EA.Audit.AuditService.Application.Features.Shared;
using EA.Audit.AuditService.Data;
using EA.Audit.AuditService.Infrastructure.Idempotency;
using EA.Audit.AuditService.Models;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;



namespace EA.Audit.AuditService.Application.Features.Audits.Commands
{
    /********************************************************************************
     * CONSIDER PULLING INTO SEPARATE API FOR SCALING INDEPENDENT OF READ (True CQRS)
     * SHOULD BE STATELESS i.e. only writing to the queue
     * A SEPARATE BACKGROUND PROCESS SHOULD READ FROM THE QUEUE AND
     * PERSIST TO THE MYSQL DB (Don't yet have Queues in PaaS...or the expected loads)
     * ******************************************************************************/
    public class CreateAuditCommand : IRequest<int>
    {
        public CreateAuditCommand()
        {

        }

        public CreateAuditCommand(DateTime dateCreated, int applicationId, int auditTypeId, string source, int auditLevelId, string details)
        {
            DateCreated = dateCreated;
            ApplicationId = applicationId;
            AuditTypeId = auditTypeId;
            Source = source;
            AuditLevelId = auditLevelId;
            Details = details;
        }
        public DateTime DateCreated { get; set; }
        public int ApplicationId { get; set; }
        public int AuditTypeId { get; set; }
        public string Source { get; set; }        
        public int AuditLevelId { get; set; }
        public string Details { get; set; }

    }

     public class CreateAuditValidator : AbstractValidator<CreateAuditCommand>
        {
            public CreateAuditValidator()
            {
                RuleFor(m => m.Details).NotNull().Length(0, 1000);
                RuleFor(m => m.Source).NotNull().Length(0, 500);
            }
        }
    
    public class CreateAuditCommandHandler : IRequestHandler<CreateAuditCommand, int>
    {
        private readonly AuditContext _dbContext;
        private readonly IMapper _mapper;

        public CreateAuditCommandHandler(IAuditContextFactory dbContextFactory, IMapper mapper)
        {
            _dbContext = dbContextFactory.AuditContext;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateAuditCommand request, CancellationToken cancellationToken)
        {
            var audit = _mapper.Map<CreateAuditCommand, AuditEntity>(request);

            _dbContext.Audits.Add(audit);

            await _dbContext.SaveChangesAsync().ConfigureAwait(false);

            return audit.Id;
        }
    }


    public class CreateAuditIdentifiedCommandHandler : IdentifiedCommandHandler<CreateAuditCommand, int>
    {
        public CreateAuditIdentifiedCommandHandler(
            IMediator mediator,
            IRequestManager requestManager,
            ILogger<IdentifiedCommandHandler<CreateAuditCommand, int>> logger)
            : base(mediator, requestManager, logger)
        {
        }

        protected override int CreateResultForDuplicateRequest()
        {
            return -1;                // Ignore duplicate requests for processing create audit.
        }
    }
}