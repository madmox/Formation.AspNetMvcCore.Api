using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using TodoApi.IntegrationTests.Fakes;
using TodoApi.IntegrationTests.Infrastructure;
using TodoApi.Repositories;

namespace TodoApi.IntegrationTests.Fixtures
{
    public abstract class BaseApiTests
    {
        private TestWebApplicationFactory _factory;
        private HttpClient _client;
        protected InMemoryTodoItemRepository _fakeTodoItemRepository;

        [SetUp]
        public void SetUpBaseApiTests()
        {
            this._factory = new TestWebApplicationFactory();
            this._client = this._factory
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureTestServices(this.ConfigureAllServices);
                })
                .CreateClient();
        }

        [TearDown]
        public void TearDownBaseApiTests()
        {
            if (this._client != null)
            {
                this._client.Dispose();
                this._client = null;
            }
            if (this._factory != null)
            {
                this._factory.Dispose();
                this._factory = null;
            }
        }

        private void ConfigureAllServices(IServiceCollection services)
        {
            this.RegisterGlobalFakes(services);
            this.RegisterFakes(services);
        }

        private void RegisterGlobalFakes(IServiceCollection services)
        {
            // Register fakes used in all tests here

            this._fakeTodoItemRepository = new InMemoryTodoItemRepository();
            services.AddSingleton<ITodoItemRepository>(this._fakeTodoItemRepository);
        }

        /// <summary>
        /// Registers fakes for a specific test class. The base implementation does nothing.
        /// </summary>
        /// <param name="services"></param>
        protected virtual void RegisterFakes(IServiceCollection services) { }

        // API client members & helper methods

        private readonly MediaTypeFormatter _formatter = new JsonMediaTypeFormatter() { Indent = true };

        private string GenerateUrl(string path, object[] urlParams, Dictionary<string, object> queryStringParams)
        {
            string result = string.Format(path, urlParams ?? new object[0]);

            if (queryStringParams != null)
            {
                string qs = queryStringParams
                    .Where(x => x.Value != null)
                    .Select(x => new
                    {
                        Key = Uri.EscapeDataString(x.Key),
                        Value = Uri.EscapeDataString(x.Value.ToString())
                    })
                    .Select(x => $"{x.Key}={x.Value}")
                    .Aggregate((x, y) => $"{x}&{y}");

                result = $"{result}?{qs}";
            }

            return result;
        }

        protected async Task<HttpResponseMessage> Get(string path, object[] urlParams = null, Dictionary<string, object> queryStringParams = null)
        {
            string url = this.GenerateUrl(path, urlParams, queryStringParams);
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            return await this._client.SendAsync(request);
        }

        protected async Task<T> GetResult<T>(string path, object[] urlParams = null, Dictionary<string, object> queryStringParams = null)
        {
            string url = this.GenerateUrl(path, urlParams, queryStringParams);
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            HttpResponseMessage response = await this._client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<T>(new List<MediaTypeFormatter>() { this._formatter });
        }

        protected async Task<HttpResponseMessage> Post(string path, object[] urlParams = null, Dictionary<string, object> queryStringParams = null, object body = null)
        {
            string url = this.GenerateUrl(path, urlParams, queryStringParams);
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            if (body != null)
            {
                request.Content = new ObjectContent(body.GetType(), body, this._formatter);
            }
            return await this._client.SendAsync(request);
        }

        protected async Task<T> PostResult<T>(string path, object[] urlParams = null, Dictionary<string, object> queryStringParams = null, object body = null)
        {
            string url = this.GenerateUrl(path, urlParams, queryStringParams);
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            if (body != null)
            {
                request.Content = new ObjectContent(body.GetType(), body, this._formatter);
            }
            HttpResponseMessage response = await this._client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<T>(new List<MediaTypeFormatter>() { this._formatter });
        }

        protected async Task<HttpResponseMessage> Put(string path, object[] urlParams = null, Dictionary<string, object> queryStringParams = null, object body = null)
        {
            string url = this.GenerateUrl(path, urlParams, queryStringParams);
            var request = new HttpRequestMessage(HttpMethod.Put, url);
            if (body != null)
            {
                request.Content = new ObjectContent(body.GetType(), body, this._formatter);
            }
            return await this._client.SendAsync(request);
        }

        protected async Task<T> PutResult<T>(string path, object[] urlParams = null, Dictionary<string, object> queryStringParams = null, object body = null)
        {
            string url = this.GenerateUrl(path, urlParams, queryStringParams);
            var request = new HttpRequestMessage(HttpMethod.Put, url);
            if (body != null)
            {
                request.Content = new ObjectContent(body.GetType(), body, this._formatter);
            }
            HttpResponseMessage response = await this._client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<T>(new List<MediaTypeFormatter>() { this._formatter });
        }

        protected async Task<HttpResponseMessage> Delete(string path, object[] urlParams = null, Dictionary<string, object> queryStringParams = null)
        {
            string url = this.GenerateUrl(path, urlParams, queryStringParams);
            var request = new HttpRequestMessage(HttpMethod.Delete, url);
            return await this._client.SendAsync(request);
        }

        protected async Task<T> DeleteResult<T>(string path, object[] urlParams = null, Dictionary<string, object> queryStringParams = null)
        {
            string url = this.GenerateUrl(path, urlParams, queryStringParams);
            var request = new HttpRequestMessage(HttpMethod.Delete, url);
            HttpResponseMessage response = await this._client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<T>(new List<MediaTypeFormatter>() { this._formatter });
        }
    }
}
