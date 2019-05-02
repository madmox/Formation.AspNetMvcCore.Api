using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using TodoApi.DTO.Commands;
using TodoApi.DTO.Results;
using TodoApi.Services;
using TodoApi.Services.Errors;

namespace TodoApi.Web.Controllers
{
    [Route("api/todo")]
    [ApiController]
    public class TodoController
    {
        private readonly ITodoService _todoService;

        public TodoController(ITodoService todoService)
        {
            this._todoService = todoService;
        }

        /// <summary>
        /// Crée un nouveau TODO item.
        /// </summary>
        /// <param name="command">Regroupe les informations nécessaires pour créer un nouveau TODO item</param>
        /// <returns>Le TODO item nouvellement créé</returns>
        /// <response code="201">Le TODO item a été créé avec succès</response>
        /// <response code="400">Des paramètres sont invalides</response>
        [HttpPost]
        [Route(@"")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(TodoItemDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([Required]TodoItemCreateCommand command)
        {
            try
            {
                TodoItemDTO result = await this._todoService.CreateTodoItem(command);
                return new ObjectResult(result) { StatusCode = StatusCodes.Status201Created };
            }
            catch (InvalidInputException ex)
            {
                var error = new ErrorResultDTO() { Message = ex.Message };
                return new ObjectResult(error) { StatusCode = StatusCodes.Status400BadRequest };
            }
        }

        /// <summary>
        /// Récupère un TODO item existant.
        /// </summary>
        /// <param name="id">L'identifiant du TODO item à récupérer</param>
        /// <returns>Le TODO item existant</returns>
        /// <response code="200">Le TODO item existe et est retourné dans le corps de la réponse</response>
        /// <response code="404">Le TODO item n'existe pas</response>
        [HttpGet]
        [Route(@"{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TodoItemDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                TodoItemDTO result = await this._todoService.GetTodoItem(id);
                if (result == null)
                {
                    var error = new ErrorResultDTO() { Message = "Item not found" };
                    return new ObjectResult(error) { StatusCode = StatusCodes.Status404NotFound };
                }

                return new ObjectResult(result) { StatusCode = StatusCodes.Status200OK };
            }
            catch (NotFoundException ex)
            {
                var error = new ErrorResultDTO() { Message = ex.Message };
                return new ObjectResult(error) { StatusCode = StatusCodes.Status404NotFound };
            }
        }

        /// <summary>
        /// Met à jour un TODO item existant.
        /// </summary>
        /// <param name="id">L'identifiant du TODO item à mettre à jour</param>
        /// <param name="command">Regroupe les informations nécessaires pour mettre à jour le TODO item</param>
        /// <returns>Le TODO item mis à jour</returns>
        /// <response code="200">Le TODO item a bien été mis à jour et est retourné dans le corps de la réponse</response>
        /// <response code="400">Des paramètres sont invalides</response>
        /// <response code="404">Le TODO item n'existe pas</response>
        [HttpPut]
        [Route(@"{id}")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TodoItemDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put(Guid id, [Required]TodoItemUpdateCommand command)
        {
            try
            {
                TodoItemDTO result = await this._todoService.UpdateTodoItem(id, command);
                return new ObjectResult(result) { StatusCode = StatusCodes.Status200OK };
            }
            catch (InvalidInputException ex)
            {
                var error = new ErrorResultDTO() { Message = ex.Message };
                return new ObjectResult(error) { StatusCode = StatusCodes.Status400BadRequest };
            }
            catch (NotFoundException ex)
            {
                var error = new ErrorResultDTO() { Message = ex.Message };
                return new ObjectResult(error) { StatusCode = StatusCodes.Status404NotFound };
            }
        }

        /// <summary>
        /// Supprime un TODO item existant.
        /// </summary>
        /// <param name="id">L'identifiant du TODO item à supprimer</param>
        /// <response code="204">Le TODO item a bien été supprimé</response>
        /// <response code="404">Le TODO item n'existe pas</response>
        [HttpDelete]
        [Route(@"{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await this._todoService.DeleteTodoItem(id);
                return new StatusCodeResult(StatusCodes.Status204NoContent);
            }
            catch (NotFoundException ex)
            {
                var error = new ErrorResultDTO() { Message = ex.Message };
                return new ObjectResult(error) { StatusCode = StatusCodes.Status404NotFound };
            }
        }

        /// <summary>
        /// Récupère une liste des TODO item existants.
        /// </summary>
        /// <returns>La liste des TODO item existants</returns>
        /// <response code="200">La liste des TODO items est retournée dans le corps de la réponse</response>
        [HttpGet]
        [Route(@"")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<TodoItemDTO>))]
        public async Task<IActionResult> List()
        {
            List<TodoItemDTO> result = await this._todoService.ListTodoItems();
            return new ObjectResult(result) { StatusCode = StatusCodes.Status200OK };
        }
    }
}
