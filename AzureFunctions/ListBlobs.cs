using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs.Extensions.Http;
using AzureFunctions.Domain;
using MediatR;
using Microsoft.Azure.WebJobs;
using System.Threading.Tasks;

namespace AzureFunctions
{        
    
   public class ListBlobs
   {
    private readonly IMediator _mediator;
    private readonly IHttpFunctionExecutor _httpFunctionExecutor;

    public ListBlobs(IMediator mediator, IHttpFunctionExecutor httpFunctionExecutor)
    {
        _mediator = mediator;
        _httpFunctionExecutor = httpFunctionExecutor;
    }

       [FunctionName("ListBlobs")]
       public async Task<IActionResult> Run(
           [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]
           GetFileQuery req,
           ILogger log)
       {
            log.LogInformation("C# HTTP trigger function processed a request.");
            log.LogInformation(req.Name);
            return await _httpFunctionExecutor.ExecuteAsync(async () =>
            {
                var blobStoragefiles = await _mediator.Send(req);
                return new OkObjectResult(blobStoragefiles);
            });
        }
   }
}
