using System;

namespace TodoApi.Entities
{
    public class TodoItem
    {
        public Guid Id { get; set; }
        public string Label { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        public TodoItem()
        {
            this.Id = Guid.NewGuid();
            this.Created = DateTime.UtcNow;
            this.Updated = this.Created;
        }
    }
}
