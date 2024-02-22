using System;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;

namespace AzureTask
{
	public class SimulateBlobContainer
	{

        public async Task Simulate(CloudStorageAccount storageAccount, string blobContainerName)
        {
            var blobClient = storageAccount.CreateCloudBlobClient();
            var blobContainer = blobClient.GetContainerReference(blobContainerName);
            await blobContainer.CreateIfNotExistsAsync();

            // Upload a blob
            var blobName = "example.txt";
            var blob = blobContainer.GetBlockBlobReference(blobName);
            await blob.UploadTextAsync("This is a test blob.");

            Console.WriteLine($"Blob '{blobName}' uploaded successfully.");
        }

    }
}

