using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Entities;
using TodoApi.Repositories;

namespace TodoApi.IntegrationTests.Fakes
{
    public class InMemoryTodoItemRepository : ITodoItemRepository
    {
        public Dictionary<Guid, TodoItem> AllItems { get; } = new Dictionary<Guid, TodoItem>();

        Task ITodoItemRepository.Create(TodoItem item)
        {
            if (this.AllItems.ContainsKey(item.Id))
            {
                throw new Exception("An item with the specified ID already exists");
            }
            else
            {
                this.AllItems[item.Id] = item;
            }

            return Task.CompletedTask;
        }

        Task<TodoItem> ITodoItemRepository.Get(Guid id)
        {
            this.AllItems.TryGetValue(id, out TodoItem item);

            return Task.FromResult(item);
        }

        Task ITodoItemRepository.Update(TodoItem item)
        {
            if (!this.AllItems.ContainsKey(item.Id))
            {
                throw new Exception("The item with the specified ID does not exists");
            }
            else
            {
                item.Updated = DateTime.UtcNow;
                this.AllItems[item.Id] = item;
            }

            return Task.CompletedTask;
        }

        Task<bool> ITodoItemRepository.Delete(Guid id)
        {
            bool found = this.AllItems.Remove(id);

            return Task.FromResult(found);
        }

        Task<List<TodoItem>> ITodoItemRepository.List()
        {
            var allItems = this.AllItems
                .Select(x => x.Value)
                .ToList();

            return Task.FromResult(allItems);
        }
    }
}
