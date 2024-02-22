using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Storage;

namespace AzureTask.Controllers;

[ApiController]
[Route("[controller]")]
public class AzureController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public AzureController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpGet("SimulateBlobContainer", Name = "SimulateBlobContainer")]
    public async Task<ActionResult> SimulateBlobContainer()
    {
        try
        {
            // Connect to Azurite
            var storageAccount = CloudStorageAccount.Parse("UseDevelopmentStorage=true");

            var blobContainerName = _configuration["Azurite:BlobStorageContainerName"];

            var blobContainer = new SimulateBlobContainer();
            await blobContainer.Simulate(storageAccount, blobContainerName);
            return Ok($"Azurite Blob Container with name: {blobContainerName} added");
        }
        catch (HttpRequestException ex)
        {
            return StatusCode(500, $"Error accessing Azurite Blob service: {ex.Message}");
        }
    }

    [HttpGet("SimulateQueue", Name = "SimulateQueue")]
    public async Task<ActionResult> SimulateQueue()
    {
        try
        {
            var storageAccount = CloudStorageAccount.Parse("UseDevelopmentStorage=true");

            var queueName = _configuration["Azurite:QueueName"];

            var queue = new SimulateQueue();
            await queue.Simulate(storageAccount, queueName);
            return Ok($"Azurite Queue with name: {queueName} added");
        }
        catch (HttpRequestException ex)
        {
            return StatusCode(500, $"Error accessing Azurite Queue service: {ex.Message}");
        }
    }

}
