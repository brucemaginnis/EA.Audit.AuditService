﻿using AuditService.Application.Commands;
using AuditService.Application.Queries;
using AuditService.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AuditServiceTests
{
    [TestFixture]
    public class AuditServiceWebApiTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<ILogger<AuditController>> _loggerMock;

        public AuditServiceWebApiTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _loggerMock = new Mock<ILogger<AuditController>>();
        }

        [Test]
        public async Task Create_audit_with_requestId_success()
        {
            //Arrange
            _mediatorMock.Setup(x => x.Send(It.IsAny<IdentifiedCommand<CreateAuditCommand, int>>(), default(CancellationToken)))
                .Returns(Task.FromResult(10));

            //Act
            var auditController = new AuditController(_loggerMock.Object, _mediatorMock.Object);
            var actionResult = await auditController.CreateAuditAsync(new CreateAuditCommand(), Guid.NewGuid().ToString()) as OkObjectResult;

            //Assert
            Assert.AreEqual(actionResult.StatusCode, (int)System.Net.HttpStatusCode.OK);

        }

        [Test]
        public async Task Create_audit_bad_request()
        {
            //Arrange
            _mediatorMock.Setup(x => x.Send(It.IsAny<IdentifiedCommand<CreateAuditCommand, int>>(), default(CancellationToken)))
                .Returns(Task.FromResult(10));

            //Act
            var auditController = new AuditController(_loggerMock.Object, _mediatorMock.Object);
            var actionResult = await auditController.CreateAuditAsync(new CreateAuditCommand(), String.Empty) as BadRequestResult;

            //Assert
            Assert.AreEqual(actionResult.StatusCode, (int)System.Net.HttpStatusCode.BadRequest);
        }       

        [Test]
        public async Task Get_audits_success()
        {
            //Arrange
            var fakeDynamicResult = new PagedResponse<AuditDto>();

            _mediatorMock.Setup(x => x.Send(It.IsAny<GetAuditsQuery>(), default(CancellationToken)))
                .Returns(Task.FromResult(fakeDynamicResult));

            //Act
            var auditController = new AuditController(_loggerMock.Object, _mediatorMock.Object);
            var actionResult = await auditController.GetAuditsAsync( new GetAuditsQuery() ) as OkObjectResult;

            //Assert
            Assert.AreEqual((actionResult as OkObjectResult).StatusCode, (int)System.Net.HttpStatusCode.OK);
        }

        [Test]
        public async Task Get_audit_success()
        {
            //Arrange
            var fakeDynamicResult = new AuditDto();

            _mediatorMock.Setup(x => x.Send(It.IsAny<GetAuditDetailsQuery>(), default(CancellationToken)))
                .Returns(Task.FromResult(fakeDynamicResult));

            //Act
            var auditController = new AuditController(_loggerMock.Object, _mediatorMock.Object);
            var actionResult = await auditController.GetAudit(10) as OkObjectResult;

            //Assert
            Assert.AreEqual((actionResult as OkObjectResult).StatusCode, (int)System.Net.HttpStatusCode.OK);
        }
       
    }
}
