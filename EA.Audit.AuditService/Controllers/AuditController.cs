using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MediatR;
using System.Threading.Tasks;
using EA.Audit.AuditService.Infrastructure;
using System;
using EA.Audit.AuditService.Application.Extensions;
using EA.Audit.AuditService.Application.Features.Shared;
using EA.Audit.AuditService.Application.Features.Audits.Queries;
using EA.Audit.AuditService.Application.Features.Audits.Commands;

namespace EA.Audit.AuditService.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [AllowCrossSiteJson]
    public class AuditController : ControllerBase
    { 
        private readonly ILogger<AuditController> _logger;
        private readonly IMediator _mediator;

        public AuditController(ILogger<AuditController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet(ApiRoutes.Audits.GetAll)]
        /*[Authorize]*/
        public async Task<ActionResult> GetAuditsAsync([FromQuery]GetAuditsQuery request)
        {
            //Shifted Auth to API Gateway
            //Extract the Auth Token and identify the calling app
            //Audit events will be restricted to each app            
            //var authenticateInfo = await HttpContext.AuthenticateAsync("Bearer").ConfigureAwait(false);
            //string accessToken = authenticateInfo.Properties.Items[".Token.access_token"];
            var audits = await _mediator.Send(request).ConfigureAwait(false);
            return Ok(audits);
        }

        [HttpGet(ApiRoutes.Audits.Get)]
        /*[Authorize]*/
        public async Task<ActionResult> GetAuditAsync(int id)
        { 
            var audit = await _mediator.Send(new GetAuditDetailsQuery() { Id = id });
            return Ok(audit);
        }


        /**********************************************************************
         * CONSIDER PULLING INTO SEPARATE API FOR SCALING INDEPENDENT OF READ
         * ********************************************************************/
        [HttpPost(ApiRoutes.Audits.Create)]
        /*[Authorize]*/
        public async Task<IActionResult> CreateAuditAsync([FromBody]CreateAuditCommand command, [FromHeader(Name = "x-requestid")] string requestId)
        {
            int commandResult = -1;

            if (Guid.TryParse(requestId, out Guid guid) && guid != Guid.Empty)
            {
                var requestCreateAudit = new IdentifiedCommand<CreateAuditCommand, int>(command, guid);

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


        [HttpGet(ApiRoutes.Audits.Search)]
        /*[Authorize]*/
        public async Task<ActionResult> SearchAuditsAsync([FromQuery]SearchAuditsQuery request)
        {
            var audits = await _mediator.Send(request).ConfigureAwait(false);
            return Ok(audits);
        }

    }     
}
