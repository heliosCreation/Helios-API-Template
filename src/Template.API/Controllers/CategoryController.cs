using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Template.API.Contract;
using Template.Application.Features.Categories.Commands.Create;
using Template.Application.Features.Categories.Commands.Delete;
using Template.Application.Features.Categories.Commands.Update;
using Template.Application.Features.Categories.Queries.GetCategoriesList;
using Template.Application.Features.Categories.Queries.GetCategoryWithEvent;

namespace Template.API.Controllers
{
    using static ApiRoutes.Category;
    [Authorize]
    public class CategoryController : ApiController
    {
        [HttpGet(GetAll, Name = "Get All Categories")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            var dtos = await Mediator.Send(new GetCategoriesListQuery());
            return Ok(dtos);
        }

        [HttpGet(GetById, Name = "Category with Events")]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetWithEvent(Guid id, bool includeHistory)
        {
            var dtos = await Mediator.Send(new GetCategoryWithEventQuery { Id = id, IncludeHistory = includeHistory });
            return Ok(dtos);
        }

        [HttpPost(Create)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddCategory([FromBody] CreateCategoryCommand command)
        {
            var response = await Mediator.Send(command);
            return Ok(response);
        }

        [HttpPut(Update)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Put([FromRoute] Guid id, [FromBody] UpdateCategoryCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(command));
        }

        [HttpDelete(Delete)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> DeleteCategory([FromRoute] Guid id)
        {
            return Ok(await Mediator.Send(new DeleteCategoryCommand(id)));
        }
    }
}
