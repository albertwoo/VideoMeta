namespace VideoMeta.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using VideoMeta.Api.Dtos;
using VideoMeta.Api.Services;


[ApiController]
[Route("/api/v1/themes")]
public class ThemesController : ControllerBase
{
    [HttpGet]
    public async Task<ThemeResp> GetThemes(
        [FromServices] IThemeService themeService,
        [FromQuery] ThemeReq req)
    {
        return await themeService.GetThemes(req);
    }


    [HttpGet("{id}")]
    [ProducesResponseType(200, Type = typeof(ThemeItem))]
    public async Task<IActionResult> GetTheme(
        Guid id,
        [FromServices] IThemeService themeService)
    {
        var data = await themeService.GetTheme(id);

        if (data == null)
        {
            return NotFound();
        }
        else
        {
            return Ok(data);
        }
    }
}
