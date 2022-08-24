using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace VideoMeta.Functions
{
    public class VideoMetaChangedHandler
    {
        [FunctionName("VideoMetaChangedHandler")]
        public void Run([QueueTrigger("VideoMeta", Connection = "")]string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
        }
    }
}
