using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Funkmap.Payments.Tests
{
    public class BandmapAuthorizationTest
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;

        public BandmapAuthorizationTest()
        {
            _configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build();

            var server = new TestServer(new WebHostBuilder()
                .UseStartup<Startup>()
                .ConfigureServices(x => x.AddAutofac())
                .UseConfiguration(_configuration)
            );
            _client = server.CreateClient();
        }


        [Fact]
        public async Task AuthTest()
        {
            var response = await _client.GetAsync("/api/product");
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);

            var bandmapApiUrl = _configuration["Auth:BandmapTokenUrl"];
            var bandmapLogin = _configuration["Auth:BandmapLogin"];
            var bandmapPassword = _configuration["Auth:BandmapPassword"];

            String token;

            using (var httpClient = new HttpClient())
            {
                var request = new HttpRequestMessage()
                {
                    Content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>()
                    {
                        new KeyValuePair<string, string>("username", bandmapLogin),
                        new KeyValuePair<string, string>("password", bandmapPassword),
                        new KeyValuePair<string, string>("grant_type", "password"),
                        new KeyValuePair<string, string>("client_id", "funkmap"),
                        new KeyValuePair<string, string>("client_secret", "funkmap"),
                    }),
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(bandmapApiUrl)
                };

                HttpResponseMessage loginResponse = await httpClient.SendAsync(request);
                Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);

                var contentJson = await loginResponse.Content.ReadAsStringAsync();
                token = JObject.Parse(contentJson).SelectToken("access_token").ToString();
                Assert.NotEmpty(token);
            }

            var requestMessage = new HttpRequestMessage
            {
                RequestUri = new Uri("/api/product", UriKind.Relative),
                Headers = { { "Authorization", $"Bearer {token}" } },
                Method = HttpMethod.Get
            };

            response = await _client.SendAsync(requestMessage);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
