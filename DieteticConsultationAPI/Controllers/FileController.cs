using DieteticConsultationAPI.Entities;
using DieteticConsultationAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DieteticConsultationAPI.Controllers
{
    [Route("file")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;

        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpPost]
        //[Authorize(Roles = "Admin,Dietician")]
        public IActionResult Upload(IFormFile file)
        {
            _fileService.UploadFile(file);

            return Ok();
        }

        [HttpGet("{id}")]
        //[Authorize(Roles = "Admin,Dietician,Patient")]
        public IActionResult Download(int id)
        {
            var file =  _fileService.DownloadFile(id);

            return File(file.Attachment, file.FileType, file.FileName);
        }

        [HttpDelete("{id}")]
       // [Authorize(Roles = "Admin,Dietician")]
        public IActionResult Delete(int id)
        {
            _fileService.DeleteFile(id);

            return NoContent();
        }
        
    }
}
