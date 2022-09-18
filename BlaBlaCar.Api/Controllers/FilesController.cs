using BlaBlaCar.BL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlaBlaCar.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IFileService _fileService;
        public FilesController(IFileService fileService)
        {
            _fileService = fileService;
        }

        //[HttpPost()]
        //public async Task<IActionResult> DeleteFile([FromForm] string filePath)
        //{
        //    _fileService.DeleteFileFormApi(filePath);
        //    return Ok();
        //}
    }
}
