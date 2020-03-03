using EA.Audit.AuditService.Application.Features.AuditTypes.Queries;
using EA.Audit.AuditService.Application.Features.Shared;
using EA.Audit.AuditService.AuditTypes.Commands;
using EA.Audit.AuditService.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EA.Audit.AuditService.Tests
{
    [TestFixture]
    public class AuditTypeServiceWepApiTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<ILogger<AuditTypeController>> _loggerMock;

        public AuditTypeServiceWepApiTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _loggerMock = new Mock<ILogger<AuditTypeController>>();
        }

        [Test]
        public async Task Create_auditLevel_with_requestId_success()
        {
            //Arrange
            _mediatorMock.Setup(x => x.Send(It.IsAny<IdentifiedCommand<CreateAuditTypeCommand, int>>(), default(CancellationToken)))
                .Returns(Task.FromResult(10));

            //Act
            var auditController = new AuditTypeController(_loggerMock.Object, _mediatorMock.Object);
            var actionResult = await auditController.CreateAuditTypeAsync(new CreateAuditTypeCommand(), Guid.NewGuid().ToString()) as OkObjectResult;

            //Assert
            Assert.AreEqual(actionResult.StatusCode, (int)System.Net.HttpStatusCode.OK);

        }

        [Test]
        public async Task Create_auditLevel_bad_request()
        {
            //Arrange
            _mediatorMock.Setup(x => x.Send(It.IsAny<IdentifiedCommand<CreateAuditTypeCommand, int>>(), default(CancellationToken)))
                .Returns(Task.FromResult(10));

            //Act
            var auditController = new AuditTypeController(_loggerMock.Object, _mediatorMock.Object);
            var actionResult = await auditController.CreateAuditTypeAsync(new CreateAuditTypeCommand(), String.Empty) as BadRequestResult;

            //Assert
            Assert.AreEqual(actionResult.StatusCode, (int)System.Net.HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task Get_auditLevels_success()
        {
            //Arrange
            var fakeDynamicResult = new PagedResponse<AuditTypeDto>();

            _mediatorMock.Setup(x => x.Send(It.IsAny<GetAuditTypesQuery>(), default(CancellationToken)))
                .Returns(Task.FromResult(fakeDynamicResult));

            //Act
            var auditController = new AuditTypeController(_loggerMock.Object, _mediatorMock.Object);
            var actionResult = await auditController.GetAuditTypesAsync(new GetAuditTypesQuery()) as OkObjectResult;

            //Assert
            Assert.AreEqual((actionResult as OkObjectResult).StatusCode, (int)System.Net.HttpStatusCode.OK);
        }

        [Test]
        public async Task Get_auditLevel_success()
        {
            //Arrange
            var fakeDynamicResult = new AuditTypeDto();

            _mediatorMock.Setup(x => x.Send(It.IsAny<GetAuditTypeDetailsQuery>(), default(CancellationToken)))
                .Returns(Task.FromResult(fakeDynamicResult));

            //Act
            var auditController = new AuditTypeController(_loggerMock.Object, _mediatorMock.Object);
            var actionResult = await auditController.GetAuditTypeAsync(10) as OkObjectResult;

            //Assert
            Assert.AreEqual((actionResult as OkObjectResult).StatusCode, (int)System.Net.HttpStatusCode.OK);
        }

    }
}
