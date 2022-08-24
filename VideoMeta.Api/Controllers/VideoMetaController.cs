namespace VideoMeta.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using VideoMeta.Api.Dtos;
using VideoMeta.Api.Services;


[ApiController]
[Route("/api/v1/videometas")]
public class VideoMetaController : ControllerBase
{
    [HttpGet]
    public async Task<VideoMetaResp> GetVideoMetas(
        [FromServices] IVideoMetaService videoMetaService,
        [FromQuery] VideoMetaReq req)
    {
        return await videoMetaService.GetVideoMetas(req);
    }


    [HttpGet("{id}")]
    [ProducesResponseType(200, Type = typeof(VideoMetaItem))]
    public async Task<IActionResult> GetVideoMeta(
        Guid id,
        [FromServices] IVideoMetaService videoMetaService)
    {
        var data = await videoMetaService.GetVideoMeta(id);

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
