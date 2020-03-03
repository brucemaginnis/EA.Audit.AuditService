using EA.Audit.AuditService.Application.Features.Application.Queries;
using EA.Audit.AuditService.Application.Features.AuditLevels.Queries;
using EA.Audit.AuditService.Application.Features.Shared;
using EA.Audit.AuditService.AuditLevels.Commands;
using EA.Audit.AuditService.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EA.Audit.AuditService.Tests
{
    [TestFixture]
    public class AuditLevelServiceWepApiTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<ILogger<AuditLevelController>> _loggerMock;

        public AuditLevelServiceWepApiTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _loggerMock = new Mock<ILogger<AuditLevelController>>();
        }

        [Test]
        public async Task Create_auditLevel_with_requestId_success()
        {
            //Arrange
            _mediatorMock.Setup(x => x.Send(It.IsAny<IdentifiedCommand<CreateAuditLevelCommand, int>>(), default(CancellationToken)))
                .Returns(Task.FromResult(10));

            //Act
            var auditController = new AuditLevelController(_loggerMock.Object, _mediatorMock.Object);
            var actionResult = await auditController.CreateAuditLevelAsync(new CreateAuditLevelCommand(), Guid.NewGuid().ToString()) as OkObjectResult;

            //Assert
            Assert.AreEqual(actionResult.StatusCode, (int)System.Net.HttpStatusCode.OK);

        }

        [Test]
        public async Task Create_auditLevel_bad_request()
        {
            //Arrange
            _mediatorMock.Setup(x => x.Send(It.IsAny<IdentifiedCommand<CreateAuditLevelCommand, int>>(), default(CancellationToken)))
                .Returns(Task.FromResult(10));

            //Act
            var auditController = new AuditLevelController(_loggerMock.Object, _mediatorMock.Object);
            var actionResult = await auditController.CreateAuditLevelAsync(new CreateAuditLevelCommand(), String.Empty) as BadRequestResult;

            //Assert
            Assert.AreEqual(actionResult.StatusCode, (int)System.Net.HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task Get_auditLevels_success()
        {
            //Arrange
            var fakeDynamicResult = new PagedResponse<AuditLevelDto>();

            _mediatorMock.Setup(x => x.Send(It.IsAny<GetAuditLevelsQuery>(), default(CancellationToken)))
                .Returns(Task.FromResult(fakeDynamicResult));

            //Act
            var auditController = new AuditLevelController(_loggerMock.Object, _mediatorMock.Object);
            var actionResult = await auditController.GetAuditLevelsAsync(new GetAuditLevelsQuery()) as OkObjectResult;

            //Assert
            Assert.AreEqual((actionResult as OkObjectResult).StatusCode, (int)System.Net.HttpStatusCode.OK);
        }

        [Test]
        public async Task Get_auditLevel_success()
        {
            //Arrange
            var fakeDynamicResult = new AuditLevelDto();

            _mediatorMock.Setup(x => x.Send(It.IsAny<GetAuditLevelDetailsQuery>(), default(CancellationToken)))
                .Returns(Task.FromResult(fakeDynamicResult));

            //Act
            var auditController = new AuditLevelController(_loggerMock.Object, _mediatorMock.Object);
            var actionResult = await auditController.GetAuditLevelAsync(10) as OkObjectResult;

            //Assert
            Assert.AreEqual((actionResult as OkObjectResult).StatusCode, (int)System.Net.HttpStatusCode.OK);
        }

    }
}
