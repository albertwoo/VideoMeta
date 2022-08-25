namespace VideoMeta.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using VideoMeta.Api.Dtos;
using VideoMeta.Api.Services;


[ApiController]
[Route("/api/v1/themes")]
public class ThemesController : ControllerBase
{
    private readonly IThemeService themeService;

    public ThemesController(IThemeService themeService)
    {
        this.themeService = themeService;
    }


    [HttpGet]
    public async Task<ThemeResp> GetThemes([FromQuery] ThemeReq req)
    {
        return await themeService.GetThemes(req);
    }


    [HttpGet("{id}")]
    [ProducesResponseType(200, Type = typeof(ThemeItem))]
    public async Task<IActionResult> GetTheme(Guid id)
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
