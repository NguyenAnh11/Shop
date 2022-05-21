using Microsoft.AspNetCore.Mvc;

namespace Shop.Api.Controller
{
    [ApiController]
    public class LanguageController : ControllerBase
    {
        private readonly ILanguageService _languageService;
        private readonly ILocalizationService _localizationService;
        public LanguageController(ILanguageService languageService, ILocalizationService localizationService)
        {
            _languageService = languageService;
            _localizationService = localizationService;
        }

        [HttpGet]
        [Route("/languages/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var model = await _languageService.GetLanguageByIdAsync(id);

            return Ok(model);
        }

        [HttpPost]
        [Route("/languages")]
        public async Task<IActionResult> CreateLanguage([FromBody] LanguageDto dto)
        {
            var response = await _languageService.InsertLanguageAsync(dto);

            if (!response.Success)
                return BadRequest(await _localizationService.GetResourceAsync(response.Message));

            return CreatedAtAction(nameof(GetById), new { id = response.Data }, response.Data);
        }

        [HttpPut]
        [Route("/languages/{id}")]
        public async Task<IActionResult> UpdateLanguage(int id, [FromBody] LanguageDto dto)
        {
            if (id != dto.Id)
                return BadRequest();

            var response = await _languageService.UpdateLanguageAsync(dto);

            if (!response.Success)
                return BadRequest(await _localizationService.GetResourceAsync(response.Message));

            return NoContent();
        }

        [HttpDelete]
        [Route("/languages/{id}")]
        public async Task<IActionResult> DeleteLanguage(int id)
        {
            var response = await _languageService.DeleteLanguageAsync(id);

            if (!response.Success)
                return BadRequest(await _localizationService.GetResourceAsync(response.Message));

            return NoContent();
        }

        [HttpGet]
        [Route("/languages/resources/{id}")]
        public async Task<IActionResult> GetResourceById(int id)
        {
            var model = await _localizationService.GetResourceByIdAsync(id);

            return Ok(model);   
        }

        [HttpPost]
        [Route("/languages/{languageId}/resources")]
        public async Task<IActionResult> CreateResource(int languageId, [FromBody] LocaleResourceDto dto)
        {
            if (languageId != dto.LanguageId)
                return BadRequest();

            var response = await _localizationService.InsertResourceAsync(dto);

            if (!response.Success)
                return BadRequest(await _localizationService.GetResourceAsync(response.Message));

            return CreatedAtAction(nameof(GetResourceById), new { id = response.Data }, response.Data);
        }

        [HttpPut]
        [Route("/languages/{languageId}/resources/{id}")]
        public async Task<IActionResult> UpdateResource(int languageId, int id, [FromBody] LocaleResourceDto dto)
        {
            if (languageId != dto.LanguageId)
                return BadRequest();

            if (id != dto.Id)
                return BadRequest();

            var response = await _localizationService.UpdateResourceAsync(dto);

            if (!response.Success)
                return BadRequest(await _localizationService.GetResourceAsync(response.Message));

            return NoContent();
        }

        [HttpDelete]
        [Route("/languages/resources/{id}")]
        public async Task<IActionResult> DeleteResource(int id)
        {
            await _languageService.DeleteLanguageAsync(id);

            return NoContent();
        }
    }
}
