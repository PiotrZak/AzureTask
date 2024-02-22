using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.Azure.WebJobs;
using Newtonsoft.Json.Linq;

namespace AzureFunctions
{
    public class ProcessQueueMessage
    {
        [FunctionName("ProcessQueueMessage")]
        public async Task RunAsync([QueueTrigger("queue", Connection = "")]string queueJson, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {queueJson}");

            DateTimeOffset now = DateTimeOffset.UtcNow;
            var blobPath = $"{now.Year}/{now.Month:D2}/{now.Day:D2}/{now.Hour:D2}/{now.Minute:D2}/{Guid.NewGuid()}.json";

            JObject jsonObject = JObject.Parse(queueJson);
            if (jsonObject == null)
            {
                log.LogError("Invalid JSON data received.");
                return;
            }

            // Create a BlobServiceClient using the Azurite connection string
            var blobServiceClient = new BlobServiceClient("UseDevelopmentStorage=true");

            // Get a reference to the container
            var containerName = "blobcontainer";
            var blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);
            // Get a reference to the blob
            var blobClient = blobContainerClient.GetBlobClient(blobPath);

            // Upload the message content as a blob
            using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(queueJson));
            await blobClient.UploadAsync(stream, true);

            log.LogInformation($"Message stored as blob: {blobPath}");

        }
    }
}
