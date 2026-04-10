using BackendLibrary;
using Frontend.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Frontend.Services
{
    public class KrakenService
    {
        protected readonly HttpClient _httpClient;
        protected readonly ApplicationDbContext _dbContext;


        public KrakenService(HttpClient httpClient, ApplicationDbContext dbContext)
        {
            _httpClient = httpClient;
            _dbContext = dbContext;
        }
        

        public async Task<ExternalApiResponse> GetAccountBalances()
        {
            string actionPath = "/0/private/Balance";
            // Serializing empty body because only nonce is required for this request which is handled in KrakenHttpHandler
            string emptyBody = JsonSerializer.Serialize(new Dictionary<string, string>());

            HttpResponseMessage response = await _httpClient.PostAsync(actionPath, new StringContent(emptyBody));

            string jsonResponse = await response.Content.ReadAsStringAsync();
            ExternalApiResponse apiResponse = JsonSerializer.Deserialize<ExternalApiResponse>(jsonResponse);

            //if(apiResponse == null)
            //{
            //    string[] errorMessages = JsonSerializer.Deserialize<string[]>(jsonResponse);
            //    apiResponse = new ExternalApiResponse(new object(), errorMessages);
            //}


            return apiResponse;
        }
    }
}
