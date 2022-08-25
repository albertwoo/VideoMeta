namespace VideoMeta.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using VideoMeta.Api.Dtos;
using VideoMeta.Api.Services;


[ApiController]
[Route("/api/v1/videometas")]
public class VideoMetaController : ControllerBase
{
    private readonly IVideoMetaService videoMetaService;

    public VideoMetaController(IVideoMetaService videoMetaService)
    {
        this.videoMetaService = videoMetaService;
    }


    [HttpGet]
    public async Task<VideoMetaResp> GetVideoMetas([FromQuery] VideoMetaReq req)
    {
        return await videoMetaService.GetVideoMetas(req);
    }


    [HttpGet("{id}")]
    [ProducesResponseType(200, Type = typeof(VideoMetaItem))]
    public async Task<IActionResult> GetVideoMeta(Guid id)
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
