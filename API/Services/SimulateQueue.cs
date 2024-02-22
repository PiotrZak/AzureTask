
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Queue;

namespace AzureTask
{
	public class SimulateQueue
	{
        public async Task Simulate(CloudStorageAccount storageAccount, string queueName)
        {
            var queueClient = storageAccount.CreateCloudQueueClient();
            var queue = queueClient.GetQueueReference(queueName);
            await queue.CreateIfNotExistsAsync();

            // Add a message to the queue
            var messageContent = "This is a test message.";
            var queueMessage = new CloudQueueMessage(messageContent);
            await queue.AddMessageAsync(queueMessage);

            Console.WriteLine($"Message '{messageContent}' added to the queue.");

            // Retrieve and process messages from the queue
            var retrievedMessage = await queue.GetMessageAsync();
            if (retrievedMessage != null)
            {
                Console.WriteLine($"Message retrieved from the queue: {retrievedMessage.AsString}");
                // Process the message as needed
                await queue.DeleteMessageAsync(retrievedMessage);
                Console.WriteLine("Message deleted from the queue.");
            }
            else
            {
                Console.WriteLine("No messages available in the queue.");
            }
        }
    }
}

