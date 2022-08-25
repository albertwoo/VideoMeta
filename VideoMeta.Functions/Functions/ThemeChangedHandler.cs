namespace VideoMeta.Functions;

using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Serialization;
using VideoMeta.Functions.Dtos;
using VideoMeta.Functions.Services;


public class ThemeChangedHandler
{
    private readonly JsonSerializerOptions jsonOptions;
    private readonly IThemeService themeService;

    public ThemeChangedHandler(IThemeService themeService)
    {
        jsonOptions = new JsonSerializerOptions();
        jsonOptions.Converters.Add(new JsonStringEnumConverter());

        this.themeService = themeService;
    }


    [FunctionName("ThemeChangedHandler")]
    public async Task Run([QueueTrigger("theme-changed")] string queueItem, ILogger log)
    {
        try
        {
            log.LogInformation("Event received for theme-changed");
            var evt = JsonSerializer.Deserialize<ThemeEvent>(queueItem, jsonOptions);

            log.LogInformation("Event is processing for theme-changed");
            await themeService.HandleEvent(evt);
            log.LogInformation("Event is processed successful for theme-changed");
        }
        catch (Exception ex)
        {
            log.LogError(ex, "Event processing is failed for theme-changed");
            throw;
        }
    }
}
