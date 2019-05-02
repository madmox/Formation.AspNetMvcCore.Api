using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApi.Entities;

namespace TodoApi.Repositories
{
    public class JsonFileTodoItemRepository : ITodoItemRepository
    {
        private const string JSON_FILE_NAME = "TodoItems.json";

        public JsonFileTodoItemRepository()
        {
        }

        Task ITodoItemRepository.Create(TodoItem item)
        {
            List<TodoItem> allItems = this.GetAllItems();
            if (allItems.Any(x => x.Id == item.Id))
            {
                throw new Exception("An item with this ID already exists");
            }

            allItems.Add(item);
            this.PersistAllItems(allItems);

            return Task.CompletedTask;
        }

        Task<TodoItem> ITodoItemRepository.Get(Guid id)
        {
            List<TodoItem> allItems = this.GetAllItems();
            TodoItem item = allItems.FirstOrDefault(x => x.Id == id);

            return Task.FromResult(item);
        }

        Task ITodoItemRepository.Update(TodoItem item)
        {
            List<TodoItem> allItems = this.GetAllItems();

            bool found = false;
            var updatedItems = new List<TodoItem>();
            foreach (TodoItem existingItem in allItems)
            {
                if (existingItem.Id != item.Id)
                {
                    updatedItems.Add(existingItem);
                }
                else
                {
                    item.Updated = DateTime.UtcNow;
                    updatedItems.Add(item);
                    found = true;
                }
            }

            if (found)
            {
                this.PersistAllItems(updatedItems);
            }
            else
            {
                throw new Exception("The specified item does not exist");
            }

            return Task.CompletedTask;
        }

        Task<bool> ITodoItemRepository.Delete(Guid id)
        {
            List<TodoItem> allItems = this.GetAllItems();

            int nbItemsBeforeOperation = allItems.Count;
            allItems = allItems
                .Where(x => x.Id != id)
                .ToList();
            this.PersistAllItems(allItems);
            bool removed = (allItems.Count < nbItemsBeforeOperation);

            return Task.FromResult(removed);
        }

        Task<List<TodoItem>> ITodoItemRepository.List()
        {
            List<TodoItem> allItems = this.GetAllItems();

            return Task.FromResult(allItems);
        }

        private List<TodoItem> GetAllItems()
        {
            if (!File.Exists(JSON_FILE_NAME))
            {
                return new List<TodoItem>();
            }
            else
            {
                string json = File.ReadAllText(JSON_FILE_NAME, Encoding.UTF8);
                List<TodoItem> items = JsonConvert.DeserializeObject<List<TodoItem>>(json);
                return items;
            }
        }

        private void PersistAllItems(List<TodoItem> allItems)
        {
            string json = JsonConvert.SerializeObject(allItems);
            File.WriteAllText(JSON_FILE_NAME, json, Encoding.UTF8);
        }
    }
}
