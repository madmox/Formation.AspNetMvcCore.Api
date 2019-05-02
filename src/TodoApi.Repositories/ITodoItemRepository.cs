using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApi.Entities;

namespace TodoApi.Repositories
{
    public interface ITodoItemRepository
    {
        Task Create(TodoItem item);
        Task<TodoItem> Get(Guid id);
        Task Update(TodoItem item);
        Task<bool> Delete(Guid id);
        Task<List<TodoItem>> List();
    }
}
