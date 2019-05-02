using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApi.DTO.Results;
using TodoApi.Entities;
using TodoApi.IntegrationTests.Builders;

namespace TodoApi.IntegrationTests.Fixtures.TodoController
{
    public class when_listing_todo_items : BaseApiTests
    {
        private const string ENDPOINT_URL = "/api/todo";

        [Test]
        public async Task then_items_should_be_returned()
        {
            // Arrange
            TodoItem item = TodoItemBuilder.Start(this._fakeTodoItemRepository)
                .WithLabel("TODO")
                .Build(persist: true);

            // Act
            List<TodoItemDTO> result = await this.GetResult<List<TodoItemDTO>>(
                path: ENDPOINT_URL
            );

            // Assert
            result.Should().HaveCount(1);
            result[0].Label.Should().Be(item.Label);
        }
    }
}
