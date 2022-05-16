//using Microsoft.AspNetCore.Mvc;
//using Shop.Application.Localization.Dtos;
//using Shop.Application.Localization.Services;

//namespace Shop.Api.Controller
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class LanguageController : ControllerBase
//    {
//        private readonly ILanguageService _languageService; 
//        public LanguageController(ILanguageService languageService)
//        {
//            _languageService = languageService; 
//        }
//        [HttpGet]
//        [Route("/{id}")]
//        public async Task<IActionResult> GetById(int id)
//        {
//            var model = await _languageService.GetLanguageByIdAsync(id);

//            if(model == null)
//            {
//                return NotFound();
//            }

//            return Ok(model);
//        }
//        [HttpPost]
//        [Route("/")]
//        public async Task<IActionResult> CreateLanguage([FromBody] LanguageDto dto)
//        {
//            var response = await _languageService.InsertLanguageAsync(dto); 

//            if(!response.Success)
//            {
//                return BadRequest(response.Messages);
//            }

//            return CreatedAtAction(nameof(GetById), new { id = response.Data.Id }, null);
//        }
//        [HttpPut]
//        [Route("/{id}")]
//        public async Task<IActionResult> UpdateLanguage(int id, [FromBody] LanguageDto dto)
//        {
//            var response = await _languageService.UpdateLanguageAsync(id, dto);

//            if (!response.Success)
//            {
//                return BadRequest(response.Messages);
//            }

//            return NoContent();
//        }
//        [HttpDelete]
//        [Route("/{id}")]
//        public async Task<IActionResult> DeleteLanguage(int id)
//        {
//            var response = await _languageService.DeleteLanguageAsync()
//        }
//    }
//}
