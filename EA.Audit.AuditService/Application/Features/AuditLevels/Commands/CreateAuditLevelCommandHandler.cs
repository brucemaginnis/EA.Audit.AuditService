using EA.Audit.AuditService.Application.Features.Shared;
using EA.Audit.AuditService.Data;
using EA.Audit.AuditService.Infrastructure.Idempotency;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using EA.Audit.AuditService.Models;

namespace EA.Audit.AuditService.AuditLevels.Commands
{

    public class CreateAuditLevelCommand : IRequest<int>
    {
        public CreateAuditLevelCommand()
        {

        }

        public CreateAuditLevelCommand(DateTime dateCreated, string name, string description)
        {
            DateCreated = dateCreated;
            Name = name;
            Description = description;
        }
        public DateTime DateCreated { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

    }

    public class CreateAuditLevelValidator : AbstractValidator<CreateAuditLevelCommand>
    {
        public CreateAuditLevelValidator()
        {
            RuleFor(m => m.Name).NotNull().Length(0, 500);
            RuleFor(m => m.Description).NotNull().Length(0, 500);
        }
    }

    public class CreateAuditLevelCommandHandler : IRequestHandler<CreateAuditLevelCommand, int>
    {
        private readonly AuditContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateAuditLevelCommandHandler> _logger;

        public CreateAuditLevelCommandHandler(IAuditContextFactory dbContextFactory, IMapper mapper)
        {
            _dbContext = dbContextFactory.AuditContext;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateAuditLevelCommand request, CancellationToken cancellationToken)
        {
            var level = _mapper.Map<CreateAuditLevelCommand, AuditLevel>(request);

            _dbContext.AuditLevels.Add(level);

            await _dbContext.SaveChangesAsync().ConfigureAwait(false);

            return level.Id;
        }
    }


    public class CreateAuditLevelIdentifiedCommandHandler : IdentifiedCommandHandler<CreateAuditLevelCommand, int>
    {
        public CreateAuditLevelIdentifiedCommandHandler(
            IMediator mediator,
            IRequestManager requestManager,
            ILogger<IdentifiedCommandHandler<CreateAuditLevelCommand, int>> logger)
            : base(mediator, requestManager, logger)
        {
        }

        protected override int CreateResultForDuplicateRequest()
        {
            return -1;                // Ignore duplicate requests for processing create audit.
        }
    }
}
