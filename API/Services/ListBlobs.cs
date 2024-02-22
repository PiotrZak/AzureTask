using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

public class ListBlobsFunction
{

    private readonly IMediator _mediator;
    private readonly IConfiguration _configuration;

    public ListBlobsFunction(IMediator mediator, IConfiguration configuration)
    {
        _configuration = configuration;
        _mediator = mediator;
    }

    [FunctionName("ListBlobs")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)]
        HttpRequest req,
        ILogger log)
    {
        log.LogInformation("C# HTTP trigger function processed a request.");

        // Retrieve the connection string and container name from environment variables or configuration
        string storageConnectionString = "UseDevelopmentStorage=true";

        string containerName = req.Query["containerName"];

        if (string.IsNullOrEmpty(storageConnectionString) || string.IsNullOrEmpty(containerName))
        {
            return new BadRequestObjectResult("Please provide the 'containerName' parameter in the query string.");
        }

        // Create a CloudBlobClient object using the storage connection string
        if (!CloudStorageAccount.TryParse(storageConnectionString, out CloudStorageAccount storageAccount))
        {
            return new StatusCodeResult(500);
        }

        CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

        // Retrieve a reference to the container
        CloudBlobContainer blobContainer = blobClient.GetContainerReference(containerName);

        // Check if the container exists
        if (!await blobContainer.ExistsAsync())
        {
            return new NotFoundObjectResult($"Container '{containerName}' does not exist.");
        }

        // List all blobs in the container
        BlobContinuationToken continuationToken = null;
        List<string> blobNames = new();

        do
        {
            var results = await blobContainer.ListBlobsSegmentedAsync(continuationToken);
            continuationToken = results.ContinuationToken;

            foreach (IListBlobItem item in results.Results)
            {
                if (item is CloudBlockBlob blob)
                {
                    blobNames.Add(blob.Name);
                }
            }
        } while (continuationToken != null);

        // Return the list of blob names in JSON format
        return new OkObjectResult(blobNames);
    }
}