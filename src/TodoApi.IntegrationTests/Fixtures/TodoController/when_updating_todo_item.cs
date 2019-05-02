using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NUnit.Framework;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using TodoApi.DTO.Commands;
using TodoApi.DTO.Results;
using TodoApi.Entities;
using TodoApi.IntegrationTests.Builders;

namespace TodoApi.IntegrationTests.Fixtures.TodoController
{
    public class when_updating_todo_item : BaseApiTests
    {
        private const string ENDPOINT_URL = "/api/todo/{0}";

        [Test]
        public async Task then_updating_an_existing_item_should_work()
        {
            // Arrange
            TodoItem item = TodoItemBuilder.Start(this._fakeTodoItemRepository)
                .WithLabel("TODO")
                .CreatedAt(DateTime.UtcNow.AddDays(-1))
                .UpdatedAt(DateTime.UtcNow.AddDays(-1))
                .Build(persist: true);
            var command = new TodoItemUpdateCommand()
            {
                Label = "TODO updated"
            };

            // Act
            TodoItemDTO result = await this.PutResult<TodoItemDTO>(
                path: ENDPOINT_URL,
                urlParams: new object[] { item.Id },
                body: command
            );

            // Assert
            result.Label.Should().Be(command.Label);
            result.Updated.Should().BeCloseTo(DateTime.UtcNow, precision: 1000);
        }

        [Test]
        public async Task then_updating_a_nonexisting_item_should_return_an_error()
        {
            // Arrange
            var command = new TodoItemUpdateCommand()
            {
                Label = "TODO updated"
            };

            // Act
            HttpResponseMessage result = await this.Put(
                path: ENDPOINT_URL,
                urlParams: new object[] { Guid.NewGuid() },
                body: command
            );

            // Assert
            result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Test]
        public async Task then_updating_an_existing_item_with_invalid_data_should_return_an_error()
        {
            // Arrange
            TodoItem item = TodoItemBuilder.Start(this._fakeTodoItemRepository)
                .WithLabel("TODO")
                .Build(persist: true);
            var command = new TodoItemUpdateCommand()
            {
                Label = ""
            };

            // Act
            HttpResponseMessage result = await this.Put(
                path: ENDPOINT_URL,
                urlParams: new object[] { item.Id },
                body: command
            );

            // Assert
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }
    }
}
