using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NUnit.Framework;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using TodoApi.Entities;
using TodoApi.IntegrationTests.Builders;

namespace TodoApi.IntegrationTests.Fixtures.TodoController
{
    public class when_deleting_todo_item : BaseApiTests
    {
        private const string ENDPOINT_URL = "/api/todo/{0}";

        [Test]
        public async Task then_deleting_an_existing_item_should_work()
        {
            // Arrange
            TodoItem item = TodoItemBuilder.Start(this._fakeTodoItemRepository)
                .WithLabel("TODO")
                .Build(persist: true);

            // Act
            HttpResponseMessage result = await this.Delete(
                path: ENDPOINT_URL,
                urlParams: new object[] { item.Id }
            );

            // Assert
            result.StatusCode.Should().Be(StatusCodes.Status204NoContent);
            //TODO: check the item does not exist anymore in persistence storage
        }

        [Test]
        public async Task then_deleting_a_nonexisting_item_should_return_an_error()
        {
            // Arrange
            // Nope

            // Act
            HttpResponseMessage result = await this.Delete(
                path: ENDPOINT_URL,
                urlParams: new object[] { Guid.NewGuid() }
            );

            // Assert
            result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }
    }
}
