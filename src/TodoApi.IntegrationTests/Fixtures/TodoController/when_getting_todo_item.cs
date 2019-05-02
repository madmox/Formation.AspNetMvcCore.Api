using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NUnit.Framework;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using TodoApi.DTO.Results;
using TodoApi.Entities;
using TodoApi.IntegrationTests.Builders;

namespace TodoApi.IntegrationTests.Fixtures.TodoController
{
    public class when_getting_todo_item : BaseApiTests
    {
        private const string ENDPOINT_URL = "/api/todo/{0}";

        [Test]
        public async Task then_existing_item_should_be_returned()
        {
            // Arrange
            TodoItem item = TodoItemBuilder.Start(this._fakeTodoItemRepository)
                .WithLabel("TODO")
                .Build(persist: true);

            // Act
            TodoItemDTO result = await this.GetResult<TodoItemDTO>(
                path: ENDPOINT_URL,
                urlParams: new object[] { item.Id }
            );

            // Assert
            result.Label.Should().Be(item.Label);
        }

        [Test]
        public async Task then_nonexisting_item_should_return_notfound()
        {
            // Arrange
            // Nope

            // Act
            HttpResponseMessage result = await this.Get(
                path: ENDPOINT_URL,
                urlParams: new object[] { Guid.NewGuid() }
            );

            // Assert
            result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }
    }
}
