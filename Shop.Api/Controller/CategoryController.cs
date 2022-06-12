using Microsoft.AspNetCore.Mvc;

namespace Shop.Api.Controller
{
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICategoryService _categoryService;
        private readonly ILanguageService _languageService;

        public CategoryController(
            IMediator mediator, 
            ICategoryService categoryService, 
            ILanguageService languageService)
        {
            _mediator = mediator;
            _categoryService = categoryService;
            _languageService = languageService;
        }

        [HttpPost]
        [Route("/api/categories")]
        public async Task<IActionResult> Create([FromBody] CreateCategoryCommand command)
        {
            var id = await _mediator.Send(command);

            return Ok(id);
        }

        [HttpPut]
        [Route("/api/categories/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCategoryCommand command)
        {
            if (id != command.Id)
                return BadRequest();

            var category = await _categoryService.GetCategoryByIdAsync(id, tracked: true);

            if (category == null)
                return BadRequest();

            await _mediator.Send(command);

            return NoContent();
        }

        [HttpDelete]
        [Route("/api/categories/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id, tracked: true);

            if (category == null)
                return BadRequest();

            await _mediator.Send(new DeleteCategoryCommand(category));

            return Ok();
        }

        [HttpPost]
        [Route("/api/categories/{id}/translations/{languageId}")]
        public async Task<IActionResult> SaveTranslation(int id, int languageId, [FromBody] SaveCategoryTranslationCommand command)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);

            if (category == null)
                return BadRequest();

            var language = await _languageService.GetLanguageByIdAsync(languageId);

            if (language == null)
                return BadRequest();

            command.Language = language;
            command.Category = category;
            await _mediator.Send(command);

            return Ok();
        }

        [HttpDelete]
        [Route("/api/categories/{id}/translations/{languageId}")]
        public async Task<IActionResult> DeleteTranslation(int id , int languageId)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);

            if (category == null)
                return BadRequest();

            var language = await _languageService.GetLanguageByIdAsync(languageId);

            if (language == null)
                return BadRequest();

            var command = new DeleteCategoryTranslationCommand(category, language);

            await _mediator.Send(command);

            return Ok();
        }
    }
}
