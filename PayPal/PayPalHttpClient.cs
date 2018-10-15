using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PayPal.Models;

namespace PayPal
{
    internal class PayPalHttpClient : IDisposable
    {
        private readonly HttpClient _http;
        private readonly PayPalConfigurationProvider _configurationProvider;

        private readonly string AuthUrl = "/v1/oauth2/token";
        private PayPalToken _token;
        private DateTime? _lastAuthDate;

        public PayPalHttpClient(PayPalConfigurationProvider configurationProvider)
        {
            _configurationProvider = configurationProvider;
            _http = new HttpClient()
            {
                BaseAddress = new Uri(_configurationProvider.PayPalUrl),
                Timeout = TimeSpan.FromSeconds(30)
            };
        }

        public async Task<PayPalToken> AuthorizeAsync()
        {
            var clientId = _configurationProvider.ClientId;
            var secret = _configurationProvider.ClientSecret;
            byte[] credentials = Encoding.GetEncoding("iso-8859-1").GetBytes($"{clientId}:{secret}");

            var authRequest = new HttpRequestMessage(HttpMethod.Post, AuthUrl);
            authRequest.Headers.Authorization =
                new AuthenticationHeaderValue("Basic", Convert.ToBase64String(credentials));
            authRequest.Content = new FormUrlEncodedContent(new Dictionary<string, string>()
                {{"grant_type", "client_credentials"}});

            var response = await _http.SendAsync(authRequest);
            var content = await response.Content.ReadAsStringAsync();

            var token = JsonConvert.DeserializeObject<PayPalToken>(content);
            _token = token;
            _lastAuthDate = DateTime.UtcNow;
            return _token;
        }

        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            var token = await GetTokenAsync();
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return await _http.SendAsync(request);
        }

        private async Task<string> GetTokenAsync()
        {
            if (_token == null)
            {
                await AuthorizeAsync();
            }
            else
            {
                var expirationDate = _lastAuthDate + TimeSpan.FromSeconds(_token.ExpiresIn);
                bool isExpired = expirationDate <= DateTime.UtcNow;

                if (!_lastAuthDate.HasValue || isExpired)
                {
                    await AuthorizeAsync();
                }
            }

            return _token.Token;
        }

        public void Dispose()
        {
            _http?.Dispose();
        }
    }
}