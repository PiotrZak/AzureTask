using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace AzureFunctions.Domain.Tests
{
    [TestFixture]
    public class GetFileQueryHandlerTests
    {
        [Test]
        public async Task Handle_InvalidContainerName_ReturnsNull()
        {
            // Arrange
            var request = new GetFileQuery { Name = "" }; // Empty container name
            var cancellationToken = CancellationToken.None; // Mocked cancellation token

            var loggerMock = new Mock<ILogger<GetFileQueryHandler>>(); // Mock ILogger

            var handler = new GetFileQueryHandler(loggerMock.Object);

            // Act
            var result = await handler.Handle(request, cancellationToken);

            // Assert
            Assert.IsNull(result);
        }

    }
}
