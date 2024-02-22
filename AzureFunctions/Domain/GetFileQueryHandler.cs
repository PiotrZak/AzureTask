using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AzureFunctions.Domain
{
    public class GetFileQueryHandler : IRequestHandler<GetFileQuery, IEnumerable<File>>
    {
        private readonly ILogger<GetFileQueryHandler> _logger;

        public GetFileQueryHandler(ILogger<GetFileQueryHandler> logger)
        {
            _logger = logger;
        }

        public async Task<IEnumerable<File>> Handle(GetFileQuery request, CancellationToken cancellationToken)
        {
            try
            {
                Console.WriteLine("Processing..");
                _logger.LogInformation("Processing...");
                string storageConnectionString = "UseDevelopmentStorage=true";
                var containerName = request.Name;

                if (string.IsNullOrEmpty(storageConnectionString) || string.IsNullOrEmpty(containerName))
                {
                    _logger.LogError("Please provide the 'containerName' parameter in the query string.");
                    return null;
                }

                var blobServiceClient = new BlobServiceClient(storageConnectionString);

                // Get a reference to the container
                var blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);

                // Check if the container exists
                if (!await blobContainerClient.ExistsAsync())
                {
                    _logger.LogError($"Container '{containerName}' does not exist.");
                    return null;
                }

                // List all blobs in the container
                List<File> blobNames = new();

                await foreach (var blobItem in blobContainerClient.GetBlobsAsync())
                {
                    blobNames.Add(new File { Name = blobItem.Name });
                    Console.WriteLine(blobItem.Name);
                }

                return blobNames;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                _logger.LogInformation(ex.Message);
                return null;
            }
        }
    }
}
