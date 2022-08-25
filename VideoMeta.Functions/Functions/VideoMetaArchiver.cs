namespace VideoMeta.Functions;

using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using VideoMeta.Functions.Services;


public class VideoMetaArchiver
{
    private readonly IVideoMetaService videoMetaService;

    public VideoMetaArchiver(IVideoMetaService videoMetaService)
    {
        this.videoMetaService = videoMetaService;
    }


    /// <summary>
    /// 0 0 2 * * * at 2AM every day
    /// </summary>
    /// <param name="myTimer"></param>
    /// <param name="log"></param>
    [FunctionName("VideoMetaArchiver")]
    public async Task Run([TimerTrigger("0 */1 * * * *")] TimerInfo timerInfo, ILogger log)
    {
        try
        {
            log.LogInformation("Archive video meta started");

            await videoMetaService.ArchieveData();

            log.LogInformation("Archive video meta finished");
        }
        catch (Exception ex)
        {
            log.LogError(ex, "Archive video meta failed");
            throw;
        }
    }
}
