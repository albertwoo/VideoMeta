namespace VideoMeta.Functions;

using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Serialization;
using VideoMeta.Functions.Dtos;
using VideoMeta.Functions.Services;


public class ThemeChangedHandler
{
    private const string QueueName = "theme-changed";

    private readonly JsonSerializerOptions jsonOptions;
    private readonly IThemeService themeService;

    public ThemeChangedHandler(IThemeService themeService)
    {
        jsonOptions = new JsonSerializerOptions();
        jsonOptions.Converters.Add(new JsonStringEnumConverter());

        this.themeService = themeService;
    }

    /// <summary>
    /// IsSessionsEnabled to true, so we can set viddeo ID as the sessionId to make sure we handle event in a same instance of Azure function
    /// To ensure we process the event in order
    /// </summary>
    /// <param name="queueItem"></param>
    /// <param name="log"></param>
    /// <returns></returns>
    [FunctionName("ThemeChangedHandler")]
    public async Task Run([ServiceBusTrigger(QueueName, IsSessionsEnabled = true)] string queueItem, ILogger log)
    {
        try
        {
            log.LogInformation("Event received for {}", QueueName);
            var evt = JsonSerializer.Deserialize<ThemeEvent>(queueItem, jsonOptions);

            log.LogInformation("Event is processing for {}", QueueName);
            await themeService.HandleEvent(evt);
            log.LogInformation("Event is processed successful for {}", QueueName);
        }
        catch (Exception ex)
        {
            log.LogError(ex, "Event processing is failed for {}", QueueName);
            throw;
        }
    }
}
