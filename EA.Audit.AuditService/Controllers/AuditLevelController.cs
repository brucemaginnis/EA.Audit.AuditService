using EA.Audit.AuditService.Application.Extensions;
using EA.Audit.AuditService.Application.Features.AuditLevels.Queries;
using EA.Audit.AuditService.Application.Features.Shared;
using EA.Audit.AuditService.AuditLevels.Commands;
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
    public class AuditLevelController : ControllerBase
    {
        private readonly ILogger<AuditLevelController> _logger;
        private readonly IMediator _mediator;

        public AuditLevelController(ILogger<AuditLevelController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet(ApiRoutes.AuditLevels.GetAll)]
        [Authorize("audit-api/audit_admin")]
        public async Task<ActionResult> GetAuditLevelsAsync([FromQuery]GetAuditLevelsQuery request)
        {
            var auditLevels = await _mediator.Send(request).ConfigureAwait(false);
            return Ok(auditLevels);
        }

        [HttpGet(ApiRoutes.AuditLevels.Get)]
        [Authorize("audit-api/audit_admin")]
        public async Task<ActionResult> GetAuditLevelAsync(int id)
        {
            var audit = await _mediator.Send(new GetAuditLevelDetailsQuery() { Id = id });
            return Ok(audit);
        }


        [HttpPost(ApiRoutes.AuditLevels.Create)]
        [Authorize("audit-api/audit_admin")]
        public async Task<IActionResult> CreateAuditLevelAsync([FromBody]CreateAuditLevelCommand command, [FromHeader(Name = "x-requestid")] string requestId)
        {
            int commandResult = -1;

            if (Guid.TryParse(requestId, out Guid guid) && guid != Guid.Empty)
            {
                var requestCreateAudit = new IdentifiedCommand<CreateAuditLevelCommand, int>(command, guid);

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
