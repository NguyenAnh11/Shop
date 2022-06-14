using Microsoft.AspNetCore.Mvc;

namespace Shop.Api.Controller
{
    [ApiController]
    public class LanguageController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILanguageService _languageService;
        private readonly ITranslationService _translationService;
        public LanguageController(
            IMediator mediator,
            ILanguageService languageService, 
            ITranslationService translationService)
        {
            _mediator = mediator;
            _languageService = languageService;
            _translationService = translationService;
        }

        [HttpGet]
        [Route("/api/languages/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var model = await _mediator.Send(new GetLanguageByIdQuery(id));

            return Ok(model);
        }

        [HttpPost]
        [Route("/api/languages")]
        public async Task<IActionResult> Create([FromBody] CreateLanguageCommand command)
        {
            var response = await _mediator.Send(command);

            if (!response.Success)
                return BadRequest(response.Message);

            return CreatedAtAction(nameof(Get), new { id = response.Data }, response.Data);
        }

        [HttpPut]
        [Route("/api/languages/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateLanguageCommand command)
        {
            if (id != command.Id)
                return BadRequest();

            var response = await _mediator.Send(command);

            if (!response.Success)
                return BadRequest(response.Message);

            return NoContent();
        }

        [HttpDelete]
        [Route("/api/languages/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var language = await _languageService.GetLanguageByIdAsync(id, tracked: true);

            if (language == null)
                return BadRequest();

            var response = await _mediator.Send(new DeleteLanguageCommand(language));

            if (!response.Success)
                return BadRequest(response.Message);

            return NoContent();
        }

        [HttpGet]
        [Route("/api/languages/resources/{id}")]
        public async Task<IActionResult> GetResourceById(int id)
        {
            var model = await _mediator.Send(new GetTranslationResourceQuery(id));

            return Ok(model);   
        }

        [HttpPost]
        [Route("/api/languages/{languageId}/resources")]
        public async Task<IActionResult> CreateResource(int languageId, [FromBody] CreateTranslationResourceCommand command)
        {
            if (languageId != command.LanguageId)
                return BadRequest();

            var response = await _mediator.Send(command);

            if (!response.Success)
                return BadRequest(response.Message);

            return CreatedAtAction(nameof(GetResourceById), new { id = response.Data }, response.Data);
        }

        [HttpPut]
        [Route("/api/languages/{languageId}/resources/{id}")]
        public async Task<IActionResult> UpdateResource(int languageId, int id, [FromBody] UpdateTranslationResourceCommand command)
        {
            if (languageId != command.LanguageId)
                return BadRequest();

            if (id != command.Id)
                return BadRequest();

            var language = await _languageService.GetLanguageByIdAsync(languageId);

            if (language == null)
                return BadRequest();

            var response = await _mediator.Send(command);

            if (!response.Success)
                return BadRequest(response.Message);

            return NoContent();
        }

        [HttpDelete]
        [Route("/api/languages/resources/{id}")]
        public async Task<IActionResult> DeleteResource(int id)
        {
            var translation = await _translationService.GetTranslationByIdAsync(id, tracked: true);

            if (translation == null)
                return BadRequest();

            await _mediator.Send(new DeleteTranslationResourceCommand(translation));

            return NoContent();
        }
    }
}
