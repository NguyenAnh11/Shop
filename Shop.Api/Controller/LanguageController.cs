﻿using Microsoft.AspNetCore.Mvc;

namespace Shop.Api.Controller
{
    [ApiController]
    public class LanguageController : ControllerBase
    {
        private readonly ILanguageService _languageService;
        private readonly ITranslationService _translationService;
        public LanguageController(ILanguageService languageService, ITranslationService translationService)
        {
            _languageService = languageService;
            _translationService = translationService;
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
                return BadRequest(await _translationService.GetResourceAsync(response.Message));

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
                return BadRequest(await _translationService.GetResourceAsync(response.Message));

            return NoContent();
        }

        [HttpDelete]
        [Route("/languages/{id}")]
        public async Task<IActionResult> DeleteLanguage(int id)
        {
            var language = await _languageService.GetLanguageByIdAsync(id);

            if (language == null)
                return NotFound();

            var response = await _languageService.DeleteLanguageAsync(language);

            if (!response.Success)
                return BadRequest(await _translationService.GetResourceAsync(response.Message));

            return NoContent();
        }

        [HttpGet]
        [Route("/languages/resources/{id}")]
        public async Task<IActionResult> GetResourceById(int id)
        {
            var model = await _translationService.GetResourceByIdAsync(id);

            return Ok(model);   
        }

        [HttpPost]
        [Route("/languages/{languageId}/resources")]
        public async Task<IActionResult> CreateResource(int languageId, [FromBody] LocaleResourceDto dto)
        {
            if (languageId != dto.LanguageId)
                return BadRequest();

            var response = await _translationService.InsertResourceAsync(dto);

            if (!response.Success)
                return BadRequest(await _translationService.GetResourceAsync(response.Message));

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

            var response = await _translationService.UpdateResourceAsync(dto);

            if (!response.Success)
                return BadRequest(await _translationService.GetResourceAsync(response.Message));

            return NoContent();
        }

        [HttpDelete]
        [Route("/languages/resources/{id}")]
        public async Task<IActionResult> DeleteResource(int id)
        {
            var resource = await _translationService.GetResourceByIdAsync(id);

            if (resource == null)
                return NotFound();

            await _translationService.DeleteResourceAsync(resource);

            return NoContent();
        }
    }
}
