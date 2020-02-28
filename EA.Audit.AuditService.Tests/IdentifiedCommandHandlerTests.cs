using EA.Audit.AuditService.Application.Features.Audits.Commands;
using EA.Audit.AuditService.Application.Features.Shared;
using EA.Audit.AuditService.Infrastructure.Idempotency;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EA.Audit.AuditServiceTests.Application
{
    [TestFixture]
    public class IdentifiedCommandHandlerTest
    {
        private readonly Mock<IRequestManager> _requestManager;
        private readonly Mock<IMediator> _mediator;
        private readonly Mock<ILogger<IdentifiedCommandHandler<CreateAuditCommand, int>>> _loggerMock;

        public IdentifiedCommandHandlerTest()
        {
            _requestManager = new Mock<IRequestManager>();
            _mediator = new Mock<IMediator>();
            _loggerMock = new Mock<ILogger<IdentifiedCommandHandler<CreateAuditCommand, int>>>();
        }

        [Test]
        public async Task Handler_sends_command_when_audit_not_exist()
        {
            // Arrange
            var fakeGuid = Guid.NewGuid();
            var fakeAuditCmd = new IdentifiedCommand<CreateAuditCommand, int>(FakeAuditRequest(), fakeGuid);

            _requestManager.Setup(x => x.ExistAsync(It.IsAny<Guid>()))
               .Returns(Task.FromResult(false));

            _mediator.Setup(x => x.Send(It.IsAny<IRequest<int>>(), default(CancellationToken)))
               .Returns(Task.FromResult(10));

            //Act
            var handler = new IdentifiedCommandHandler<CreateAuditCommand, int>(_mediator.Object, _requestManager.Object, _loggerMock.Object);
            var cltToken = new CancellationToken();
            var result = await handler.Handle(fakeAuditCmd, cltToken);

            //Assert
            Assert.True(result == 10);
            _mediator.Verify(x => x.Send(It.IsAny<IRequest<int>>(), default(CancellationToken)), Times.Once());
        }

        [Test]
        public async Task Handler_sends_no_command_when_audit_already_exists()
        {
            // Arrange
            var fakeGuid = Guid.NewGuid();
            var fakeAuditCmd = new IdentifiedCommand<CreateAuditCommand, int>(FakeAuditRequest(), fakeGuid);

            _requestManager.Setup(x => x.ExistAsync(It.IsAny<Guid>()))
               .Returns(Task.FromResult(true));

            _mediator.Setup(x => x.Send(It.IsAny<IRequest<bool>>(), default(CancellationToken)))
               .Returns(Task.FromResult(true));

            //Act
            var handler = new IdentifiedCommandHandler<CreateAuditCommand, int>(_mediator.Object, _requestManager.Object, _loggerMock.Object);
            var cltToken = new CancellationToken();
            var result = await handler.Handle(fakeAuditCmd, cltToken);

            //Assert
            Assert.False(result == -1);
            _mediator.Verify(x => x.Send(It.IsAny<IRequest<bool>>(), default(CancellationToken)), Times.Never());
        }

        private CreateAuditCommand FakeAuditRequest(Dictionary<string, object> args = null)
        {
            return new CreateAuditCommand(
                dateCreated: args != null && args.ContainsKey("dateCreated") ? (DateTime)args["dateCreated"] : DateTime.UtcNow,
                applicationId: args != null && args.ContainsKey("applicationId") ? (int)args["applicationId"] : 0,
                auditLevelId: args != null && args.ContainsKey("auditLevelId") ? (int)args["auditLevelId"] : 0,
                source: args != null && args.ContainsKey("source") ? (string)args["source"] : null,
                auditTypeId: args != null && args.ContainsKey("auditTypeId") ? (int)args["auditTypeId"] : 0,
                details: args != null && args.ContainsKey("details") ? (string)args["details"] : null);
               
        }
    }
}
