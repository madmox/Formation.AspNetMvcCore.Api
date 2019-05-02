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
    public class when_creating_todo_item : BaseApiTests
    {
        private const string ENDPOINT_URL = "/api/todo";

        [Test]
        public async Task then_a_valid_command_should_create_an_item()
        {
            // Arrange
            var command = new TodoItemCreateCommand()
            {
                Label = "TODO"
            };

            // Act
            TodoItemDTO result = await this.PostResult<TodoItemDTO>(
                path: ENDPOINT_URL,
                body: command
            );

            // Assert
            result.Label.Should().Be(command.Label);
        }

        [Test]
        public async Task then_copying_an_existing_item_should_work()
        {
            // Arrange
            TodoItem item = TodoItemBuilder.Start(this._fakeTodoItemRepository)
                .WithLabel("TODO")
                .Build(persist: true);
            var command = new TodoItemCreateCommand()
            {
                CopyFrom = item.Id
            };

            // Act
            TodoItemDTO result = await this.PostResult<TodoItemDTO>(
                path: ENDPOINT_URL,
                body: command
            );

            // Assert
            result.Id.Should().NotBe(item.Id);
            result.Label.Should().Be(item.Label);
        }

        [Test]
        public async Task then_submitting_an_invalid_command_should_return_an_error()
        {
            // Arrange
            var command = new TodoItemCreateCommand()
            {
            };

            // Act
            HttpResponseMessage result = await this.Post(
                path: ENDPOINT_URL,
                body: command
            );

            // Assert
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Test]
        public async Task then_copying_a_nonexistant_item_should_return_an_error()
        {
            // Arrange
            var command = new TodoItemCreateCommand()
            {
                CopyFrom = Guid.NewGuid()
            };

            // Act
            HttpResponseMessage result = await this.Post(
                path: ENDPOINT_URL,
                body: command
            );

            // Assert
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }
    }
}
