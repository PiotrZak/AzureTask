using NUnit.Framework;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Threading.Tasks;
using NUnit.Framework.Internal.Execution;
using Microsoft.Extensions.Logging;

namespace AzureFunctions.Tests
{
    [TestFixture]
    public class ProcessQueueMessageTests
    {
        [Test]
        public async Task ProcessQueueMessage_SuccessfullyProcessesQueueMessage()
        {
            // Arrange
            var queueJson = "{\"key\": \"value\"}"; // Sample queue message JSON
            var logger = new NullLogger<ProcessQueueMessage>(); // Mock logger

            var function = new ProcessQueueMessage();

            // Act
            await function.RunAsync(queueJson, logger);

            // Assert
            // Verify that no exception was thrown
            Assert.Pass("No exception was thrown, indicating successful processing.");
        }

        [Test]
        public async Task ProcessQueueMessage_InvalidJson_ThrowsException()
        {
            // Arrange
            var queueJson = "invalid json"; // Invalid JSON format
            var logger = new NullLogger<ProcessQueueMessage>(); // Mock logger
            var function = new ProcessQueueMessage();

            // Act & Assert
            Assert.ThrowsAsync<Newtonsoft.Json.JsonReaderException>(async () => await function.RunAsync(queueJson, logger));
        }

        // Helper class for testing logging
        public class TestLogger<T> : ILogger<T>
        {
            public string Logs { get; private set; } = "";

            public IDisposable BeginScope<TState>(TState state) => throw new NotImplementedException();

            public bool IsEnabled(LogLevel logLevel) => true;

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            {
                Logs += formatter(state, exception);
            }
        }
    }
}
