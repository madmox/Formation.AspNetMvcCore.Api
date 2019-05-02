using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApi.DTO.Commands;
using TodoApi.DTO.Results;

namespace TodoApi.Services
{
    public interface ITodoService
    {
        Task<TodoItemDTO> CreateTodoItem(TodoItemCreateCommand command);
        Task<TodoItemDTO> GetTodoItem(Guid id);
        Task<TodoItemDTO> UpdateTodoItem(Guid id, TodoItemUpdateCommand command);
        Task DeleteTodoItem(Guid id);
        Task<List<TodoItemDTO>> ListTodoItems();
    }
}
