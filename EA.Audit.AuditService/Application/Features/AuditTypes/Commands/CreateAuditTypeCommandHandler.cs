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

namespace EA.Audit.AuditService.AuditTypes.Commands
{

    public class CreateAuditTypeCommand : IRequest<int>
    {
        public CreateAuditTypeCommand()
        {

        }

        public CreateAuditTypeCommand(DateTime dateCreated, string name, string description)
        {
            DateCreated = dateCreated;
            Name = name;
            Description = description;
        }
        public DateTime DateCreated { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

    }

    public class CreateAuditTypeValidator : AbstractValidator<CreateAuditTypeCommand>
    {
        public CreateAuditTypeValidator()
        {
            RuleFor(m => m.Name).NotNull().Length(0, 500);
            RuleFor(m => m.Description).NotNull().Length(0, 500);
        }
    }

    public class CreateAuditTypeCommandHandler : IRequestHandler<CreateAuditTypeCommand, int>
    {
        private readonly AuditContext _dbContext;
        private readonly IMapper _mapper;

        public CreateAuditTypeCommandHandler(IAuditContextFactory dbContextFactory, IMapper mapper)
        {
            _dbContext = dbContextFactory.AuditContext;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateAuditTypeCommand request, CancellationToken cancellationToken)
        {
            var type = _mapper.Map<CreateAuditTypeCommand, AuditType>(request);

            _dbContext.AuditTypes.Add(type);

            await _dbContext.SaveChangesAsync().ConfigureAwait(false);

            return type.Id;
        }
    }


    public class CreateAuditTypeIdentifiedCommandHandler : IdentifiedCommandHandler<CreateAuditTypeCommand, int>
    {
        public CreateAuditTypeIdentifiedCommandHandler(
            IMediator mediator,
            IRequestManager requestManager,
            ILogger<IdentifiedCommandHandler<CreateAuditTypeCommand, int>> logger)
            : base(mediator, requestManager, logger)
        {
        }

        protected override int CreateResultForDuplicateRequest()
        {
            return -1;                // Ignore duplicate requests for processing create audit.
        }
    }
}
