using System.Net.Mime;
using BlaBlaCar.BL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlaBlaCar.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriverFilesController : ControllerBase
    {
        private readonly IFileService _fileService;
        public DriverFilesController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpPost]
        public async Task<IActionResult> GetFile([FromForm] string img)
        {
            try
            {
                var res = await _fileService.GetFileAsync(img);
                return Ok(File(res, MediaTypeNames.Image.Jpeg));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
