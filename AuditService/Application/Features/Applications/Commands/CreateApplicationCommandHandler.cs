using AuditService.Application.Features.Shared;
using AuditService.Data;
using AuditService.Infrastructure.Idempotency;
using AuditService.Model.Admin;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AuditService.Application.Commands
{

    public class CreateAuditApplicationCommand : IRequest<int>
    {
        public CreateAuditApplicationCommand()
        {

        }

        public CreateAuditApplicationCommand(DateTime dateCreated, string name, string description)
        {
            DateCreated = dateCreated;
            Name = name;
            Description = description;
        }
        public DateTime DateCreated { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

    }

    public class CreateApplicationValidator : AbstractValidator<CreateAuditApplicationCommand>
    {
        public CreateApplicationValidator()
        {
            RuleFor(m => m.Name).NotNull().Length(0, 500);
            RuleFor(m => m.Description).NotNull().Length(0, 500);
        }
    }

    public class CreateApplicationCommandHandler : IRequestHandler<CreateAuditApplicationCommand, int>
    {
        private readonly AuditContext _dbContext;
        private readonly IMapper _mapper;

        public CreateApplicationCommandHandler(AuditContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateAuditApplicationCommand request, CancellationToken cancellationToken)
        {
            var app = _mapper.Map<CreateAuditApplicationCommand, AuditApplication>(request);

            _dbContext.AuditApplications.Add(app);

            await _dbContext.SaveChangesAsync();

            return app.Id;
        }
    }


    public class CreateApplicationIdentifiedCommandHandler : IdentifiedCommandHandler<CreateAuditApplicationCommand, int>
    {
        public CreateApplicationIdentifiedCommandHandler(
            IMediator mediator,
            IRequestManager requestManager,
            ILogger<IdentifiedCommandHandler<CreateAuditApplicationCommand, int>> logger)
            : base(mediator, requestManager, logger)
        {
        }

        protected override int CreateResultForDuplicateRequest()
        {
            return -1;                // Ignore duplicate requests for processing create audit.
        }
    }
}
