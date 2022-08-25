namespace VideoMeta.Functions;

using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Serialization;
using VideoMeta.Functions.Dtos;
using VideoMeta.Functions.Services;


public class VideoMetaChangedHandler
{
    private readonly JsonSerializerOptions jsonOptions;
    private readonly IVideoMetaService videoMetaService;

    public VideoMetaChangedHandler(IVideoMetaService videoMetaService)
    {
        jsonOptions = new JsonSerializerOptions();
        jsonOptions.Converters.Add(new JsonStringEnumConverter());

        this.videoMetaService = videoMetaService;
    }


    [FunctionName("VideoMetaChangedHandler")]
    public async Task Run([QueueTrigger("video-meta-changed")] string queueItem, ILogger log)
    {
        try
        {
            log.LogInformation("Event received for video-meta-changed");
            var evt = JsonSerializer.Deserialize<VideoMetaEvent>(queueItem, jsonOptions);

            log.LogInformation("Event is processing for video-meta-changed");
            await videoMetaService.HandleEvent(evt);
            log.LogInformation("Event is processed successful for video-meta-changed");
        }
        catch (Exception ex)
        {
            log.LogError(ex, "Event processing is failed for video-meta-changed");
            throw;
        }
    }
}
