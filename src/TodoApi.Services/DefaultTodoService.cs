using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.DTO.Commands;
using TodoApi.DTO.Results;
using TodoApi.Entities;
using TodoApi.Repositories;
using TodoApi.Services.Errors;

namespace TodoApi.Services
{
    public class DefaultTodoService : ITodoService
    {
        private readonly ITodoItemRepository _todoItemRepository;

        public DefaultTodoService(ITodoItemRepository todoItemRepository)
        {
            this._todoItemRepository = todoItemRepository;
        }

        async Task<TodoItemDTO> ITodoService.CreateTodoItem(TodoItemCreateCommand command)
        {
            TodoItem sourceItem = await this.Validate(command);
            var item = new TodoItem();
            command.MergeIntoEntity(item, sourceItem);
            await this._todoItemRepository.Create(item);
            return new TodoItemDTO(item);
        }

        async Task<TodoItemDTO> ITodoService.GetTodoItem(Guid id)
        {
            TodoItem item = await this._todoItemRepository.Get(id);
            if (item == null)
            {
                throw new NotFoundException("The item does not exist");
            }
            return new TodoItemDTO(item);
        }

        async Task<TodoItemDTO> ITodoService.UpdateTodoItem(Guid id, TodoItemUpdateCommand command)
        {
            await this.Validate(command);
            TodoItem item = await this._todoItemRepository.Get(id);
            if (item == null)
            {
                throw new NotFoundException("The item does not exist");
            }
            command.MergeIntoEntity(item);
            await this._todoItemRepository.Update(item);
            return new TodoItemDTO(item);
        }

        async Task ITodoService.DeleteTodoItem(Guid id)
        {
            bool deleted = await this._todoItemRepository.Delete(id);
            if (!deleted)
            {
                throw new NotFoundException("The item does not exist");
            }
        }

        async Task<List<TodoItemDTO>> ITodoService.ListTodoItems()
        {
            List<TodoItem> items = await this._todoItemRepository.List();
            return items
                .Select(x => new TodoItemDTO(x))
                .ToList();
        }

        private async Task<TodoItem> Validate(TodoItemCreateCommand command)
        {
            if (string.IsNullOrEmpty(command.Label) && !command.CopyFrom.HasValue)
            {
                throw new InvalidInputException("Both label and copy_from fields cannot be empty");
            }
            if (!string.IsNullOrEmpty(command.Label) && command.CopyFrom.HasValue)
            {
                throw new InvalidInputException("Both label and copy_from fields cannot be provided");
            }

            TodoItem sourceItem = null;
            if (command.CopyFrom.HasValue)
            {
                sourceItem = await this._todoItemRepository.Get(command.CopyFrom.Value);
                if (sourceItem == null)
                {
                    throw new InvalidInputException($"Item {command.CopyFrom.Value} does not exist");
                }
            }
            return sourceItem;
        }

        private Task Validate(TodoItemUpdateCommand command)
        {
            // Rien à valider, les annotations du DTO sont suffisantes
            return Task.CompletedTask;
        }
    }
}
