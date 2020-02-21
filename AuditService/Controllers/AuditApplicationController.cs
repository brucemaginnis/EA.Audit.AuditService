using AuditService.Application.Commands;
using AuditService.Application.Extensions;
using AuditService.Application.Features.Application.Queries;
using AuditService.Application.Features.Shared;
using AuditService.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace AuditService.Controllers
{
    [ApiController]
    [Produces("application/json")]
    public class AuditApplicationController : ControllerBase
    {
        private readonly ILogger<AuditApplicationController> _logger;
        private readonly IMediator _mediator;

        public AuditApplicationController(ILogger<AuditApplicationController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet(ApiRoutes.AuditApplications.GetAll)]
        /*[Authorize]*/
        public async Task<ActionResult> GetAuditApplicationsAsync([FromQuery]GetAuditApplicationsQuery request)
        {
            var audits = await _mediator.Send(request).ConfigureAwait(false);
            return Ok(audits);
        }

        [HttpGet(ApiRoutes.AuditApplications.Get)]
        /*[Authorize]*/
        public async Task<ActionResult> GetAuditApplicationAsync(int id)
        {
            var audit = await _mediator.Send(new GetAuditApplicationDetailsQuery() { Id = id });
            return Ok(audit);
        }

        [HttpPost(ApiRoutes.AuditApplications.Create)]
        /*[Authorize]*/
        public async Task<IActionResult> CreateAuditApplicationAsync([FromBody]CreateAuditApplicationCommand command, [FromHeader(Name = "x-requestid")] string requestId)
        {
            int commandResult = -1;

            if (Guid.TryParse(requestId, out Guid guid) && guid != Guid.Empty)
            {
                var requestCreateAuditApplication = new IdentifiedCommand<CreateAuditApplicationCommand, int>(command, guid);

                _logger.LogInformation(
                    "----- Sending command: {CommandName} - ({@Command})",
                    requestCreateAuditApplication.GetGenericTypeName(),
                    requestCreateAuditApplication);

                commandResult = await _mediator.Send(requestCreateAuditApplication);
            }

            if (commandResult == -1)
            {
                return BadRequest();
            }

            return Ok(commandResult);
        }

    }
}
