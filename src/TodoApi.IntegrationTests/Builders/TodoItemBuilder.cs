using Newtonsoft.Json;
using System;
using TodoApi.Entities;
using TodoApi.IntegrationTests.Fakes;

namespace TodoApi.IntegrationTests.Builders
{
    public class TodoItemBuilder
    {
        private readonly InMemoryTodoItemRepository _repository;

        private TodoItem _item = new TodoItem();

        private TodoItemBuilder(InMemoryTodoItemRepository fakeTodoItemRepository)
        {
            this._repository = fakeTodoItemRepository;
        }

        public TodoItemBuilder WithId(Guid id)
        {
            this._item.Id = id;
            return this;
        }

        public TodoItemBuilder WithLabel(string label)
        {
            this._item.Label = label;
            return this;
        }

        public TodoItemBuilder CreatedAt(DateTime created)
        {
            this._item.Created = created;
            return this;
        }

        public TodoItemBuilder UpdatedAt(DateTime updated)
        {
            this._item.Updated = updated;
            return this;
        }

        public static TodoItemBuilder Start(InMemoryTodoItemRepository fakeTodoItemRepository)
        {
            return new TodoItemBuilder(fakeTodoItemRepository);
        }

        public TodoItem Build(bool persist)
        {
            if (persist)
            {
                this._repository.AllItems[this._item.Id] = this.DeepCopy(this._item);
            }
            return this._item;
        }

        private TodoItem DeepCopy(TodoItem item)
        {
            string json = JsonConvert.SerializeObject(item);
            return JsonConvert.DeserializeObject<TodoItem>(json);
        }
    }
}
