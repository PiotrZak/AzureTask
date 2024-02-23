using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Queue;

[Route("[controller]")]
[ApiController]
public class AzureFunctions: ControllerBase
{

    private readonly IMediator _mediator;
    private readonly CloudQueueClient _queueClient;
    private readonly IHttpClientFactory _clientFactory;

    public AzureFunctions(IMediator mediator, IHttpClientFactory clientFactory)
    {
        _mediator = mediator;
        _clientFactory = clientFactory;
        var storageAccount = CloudStorageAccount.Parse("UseDevelopmentStorage=true");
        _queueClient = storageAccount.CreateCloudQueueClient();
    }

    [HttpGet]
    public async Task<IActionResult> TriggerAzureFunction()
    {
        // Replace <function_url> with the URL of your Azure Function
        var functionUrl = "http://localhost:7071/api/ListBlobs?Name=blobcontainer";

        try
        {
            var client = _clientFactory.CreateClient();
            var response = await client.PostAsync(functionUrl, null);

            if (response.IsSuccessStatusCode)
            {
                return Ok("Azure Function triggered successfully.");
            }
            else
            {
                return StatusCode((int)response.StatusCode, "Failed to trigger Azure Function.");
            }
        }
        catch (HttpRequestException ex)
        {
            return StatusCode(500, $"Error triggering Azure Function: {ex.Message}");
        }
    }

    [HttpPost]
    public async Task<IActionResult> SendMessageToQueue()
    {
        try
        {
            string messageContent = "{\"key1\":\"value1\",\"key2\":\"value2\"}";

            var queueName = "queue";
            var queue = _queueClient.GetQueueReference(queueName);
            await queue.CreateIfNotExistsAsync();

            // Add the message to the queue
            var queueMessage = new CloudQueueMessage(messageContent);
            await queue.AddMessageAsync(queueMessage);

            return Ok($"Message '{messageContent}' added to the queue.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error sending message to queue: {ex.Message}");
        }
    }
}


