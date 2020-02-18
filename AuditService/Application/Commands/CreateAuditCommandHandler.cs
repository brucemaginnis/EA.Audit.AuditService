using AuditService.Data;
using AuditService.Infrastructure.Idempotency;
using AuditService.Models;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;



namespace AuditService.Application.Commands
{
    public class CreateAuditCommand : IRequest<int>
    {
        public CreateAuditCommand()
        {

        }

        public CreateAuditCommand(DateTime dateCreated, int auditTypeId, string source, int auditLevelId, string details)
        {
            DateCreated = dateCreated;
            AuditTypeId = auditTypeId;
            Source = source;
            AuditLevelId = auditLevelId;
            Details = details;
        }
        public DateTime DateCreated { get; set; }        
        public int AuditTypeId { get; set; }
        public string Source { get; set; }        
        public int AuditLevelId { get; set; }
        public string Details { get; set; }

    }

     public class Validator : AbstractValidator<CreateAuditCommand>
        {
            public Validator()
            {
                RuleFor(m => m.Details).NotNull().Length(0, 1000);
                RuleFor(m => m.Source).NotNull().Length(0, 500);
            }
        }
    
    public class CreateAuditCommandHandler : IRequestHandler<CreateAuditCommand, int>
    {
        private readonly AuditContext _dbContext;
        private readonly IMapper _mapper;

        public CreateAuditCommandHandler(AuditContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateAuditCommand request, CancellationToken cancellationToken)
        {
            var audit = _mapper.Map<CreateAuditCommand, Audit>(request);

            _dbContext.Audits.Add(audit);

            await _dbContext.SaveChangesAsync();

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