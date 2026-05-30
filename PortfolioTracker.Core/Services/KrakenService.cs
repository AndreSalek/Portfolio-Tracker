using BackendLibrary;
using BackendLibrary.Interfaces;
using Frontend.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Frontend.Services
{
    public class KrakenService : IPlatformService
    {
        private const string _publicEndpointPath = "/0/public";
        private const string _privateEndpointPath = "/0/private";

        protected readonly HttpClient _httpClient;
        protected readonly ApplicationDbContext _dbContext;
        private ILogger<KrakenService> _logger;

        public KrakenService(HttpClient httpClient, ApplicationDbContext dbContext, ILogger<KrakenService> logger)
        {
            _httpClient = httpClient;
            _dbContext = dbContext;
            _logger = logger;
        }
        

        public async Task<IPlatformBalances> GetAccountBalances()
        {
            string endpoint = "/Balance";

            IExternalApiResponse apiResponse = await QueryPrivateEndpoint(endpoint, new Dictionary<string, string>(), HttpMethod.Post);

            apiResponse.Result ??= new Dictionary<string, object>();

            //IPlatformBalances balances = apiResponse.Result as PlatformBalances
            //    ?? new PlatformBalances(Platform.Kraken, new Dictionary<string, object>());
            //balances.Platform = Platform.Kraken;
            Dictionary<string, object> balances = JsonSerializer.Deserialize<Dictionary<string, object>>(apiResponse.Result.ToString());

            if (apiResponse.Error.Length > 0) _logger.LogError("Error fetching account balances from Kraken: {Errors}", string.Join(", ", apiResponse.Error));

            return new PlatformBalances(Platform.Kraken, balances);
        }

        protected async Task<IExternalApiResponse> QueryPrivateEndpoint(string endpoint, Dictionary<string, string> parameters, HttpMethod httpMethod)
        {
            string actionPath = _privateEndpointPath + endpoint;
            HttpRequestMessage httpMessage = new HttpRequestMessage(httpMethod, actionPath);

            HttpResponseMessage response = await _httpClient.SendAsync(httpMessage);

            string jsonResponse = await response.Content.ReadAsStringAsync();
            IExternalApiResponse apiResponse = JsonSerializer.Deserialize<ExternalApiResponse>(jsonResponse, JsonSerializerOptions.Web);

            return apiResponse;
        }
    }
}
