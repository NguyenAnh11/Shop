using Microsoft.AspNetCore.Mvc;

namespace Shop.Api.Controller
{
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly IPictureService _pictureService;

        public UploadController(IPictureService pictureService)
        {
            _pictureService = pictureService;
        }

        [HttpPost]
        [Route("/api/upload")]
        public async Task<IActionResult> Upload([FromForm] IFormFile file)
        {
            var picture = await _pictureService.SaveAsync(file);  
            
            return Ok(picture); 
        }
    }
}
