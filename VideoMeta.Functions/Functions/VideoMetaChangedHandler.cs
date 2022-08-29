namespace VideoMeta.Functions;

using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Serialization;
using VideoMeta.Functions.Dtos;
using VideoMeta.Functions.Services;


public class VideoMetaChangedHandler
{
    private const string QueueName = "video-meta-changed";

    private readonly JsonSerializerOptions jsonOptions;
    private readonly IVideoMetaService videoMetaService;

    public VideoMetaChangedHandler(IVideoMetaService videoMetaService)
    {
        jsonOptions = new JsonSerializerOptions();
        jsonOptions.Converters.Add(new JsonStringEnumConverter());

        this.videoMetaService = videoMetaService;
    }


    /// <summary>
    /// IsSessionsEnabled to true, so we can set viddeo ID as the sessionId to make sure we handle event in a same instance of Azure function
    /// To ensure we process the event in order
    /// </summary>
    /// <param name="queueItem"></param>
    /// <param name="log"></param>
    /// <returns></returns>
    [FunctionName("VideoMetaChangedHandler")]
    public async Task Run([ServiceBusTrigger(QueueName, IsSessionsEnabled = true)] string queueItem, ILogger log)
    {
        try
        {
            log.LogInformation("Event received for {}", QueueName);
            var evt = JsonSerializer.Deserialize<VideoMetaEvent>(queueItem, jsonOptions);

            log.LogInformation("Event is processing for {}", QueueName);
            await videoMetaService.HandleEvent(evt);
            log.LogInformation("Event is processed successful for {}", QueueName);
        }
        catch (Exception ex)
        {
            log.LogError(ex, "Event processing is failed for {}", QueueName);
            throw;
        }
    }
}
