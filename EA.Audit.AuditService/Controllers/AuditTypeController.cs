using EA.Audit.AuditService.Application.Extensions;
using EA.Audit.AuditService.Application.Features.AuditTypes.Queries;
using EA.Audit.AuditService.Application.Features.Shared;
using EA.Audit.AuditService.AuditTypes.Commands;
using EA.Audit.AuditService.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace EA.Audit.AuditService.Controllers
{

    [ApiController]
    [Produces("application/json")]
    [AllowCrossSiteJson]
    public class AuditTypeController : ControllerBase
    {
        private readonly ILogger<AuditTypeController> _logger;
        private readonly IMediator _mediator;

        public AuditTypeController(ILogger<AuditTypeController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet(ApiRoutes.AuditTypes.GetAll)]
        [Authorize("audit-api/audit_admin")]
        public async Task<ActionResult> GetAuditTypesAsync([FromQuery]GetAuditTypesQuery request)
        {
            var auditLevels = await _mediator.Send(request).ConfigureAwait(false);
            return Ok(auditLevels);
        }

        [HttpGet(ApiRoutes.AuditTypes.Get)]
        [Authorize("audit-api/audit_admin")]
        public async Task<ActionResult> GetAuditTypeAsync(int id)
        {
            var audit = await _mediator.Send(new GetAuditTypeDetailsQuery() { Id = id }).ConfigureAwait(false);
            return Ok(audit);
        }


        [HttpPost(ApiRoutes.AuditTypes.Create)]
        [Authorize("audit-api/audit_admin")]
        public async Task<IActionResult> CreateAuditTypeAsync([FromBody]CreateAuditTypeCommand command, [FromHeader(Name = "x-requestid")] string requestId)
        {
            int commandResult = -1;

            if (Guid.TryParse(requestId, out Guid guid) && guid != Guid.Empty)
            {
                var requestCreateAudit = new IdentifiedCommand<CreateAuditTypeCommand, int>(command, guid);

                _logger.LogInformation(
                    "----- Sending command: {CommandName} - ({@Command})",
                    requestCreateAudit.GetGenericTypeName(),
                    requestCreateAudit);

                commandResult = await _mediator.Send(requestCreateAudit);
            }

            if (commandResult == -1)
            {
                return BadRequest();
            }

            return Ok(commandResult);
        }
    }
}
