using BackendLibrary;
using Frontend.Data;
using Frontend.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Frontend.Common
{
    public class KrakenHttpHandler : DelegatingHandler
    {
        private UserManager<ApplicationUser> _userManager;
        private IHttpContextAccessor _httpContextAccessor;
        private ApplicationDbContext _dbContext;
        private ILogger<KrakenHttpHandler> _logger;
        public KrakenHttpHandler(UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor, ILogger<KrakenHttpHandler> logger)
        {
            _userManager = userManager;   
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;
            _logger = logger;
        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if(request.Method == HttpMethod.Get) return await base.SendAsync(request, cancellationToken);

            long nonce = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            Dictionary<string, object> postData = new Dictionary<string, object>();
            postData.Add("nonce", nonce);

            string currentBody = request.Content is null ? string.Empty : await request.Content.ReadAsStringAsync(cancellationToken);
            

            // Get the user that is making the request
            ClaimsPrincipal userIdClaim = _httpContextAccessor.HttpContext!.User;
            ApplicationUser user = await _userManager.GetUserAsync(userIdClaim) ?? throw new KeyNotFoundException("Missing user ID claim.");
            user.PlatformKeys = await _dbContext.PlatformKeys.Where(key => key.UserId == user.Id).ToListAsync();

            // Find user's key for Kraken platform else throw exception
            PlatformKey krakenKey = user.PlatformKeys.Where(key => key.Platform == Platform.Kraken).FirstOrDefault() 
                ?? throw new KeyNotFoundException("User does not have any PlatformKey for Kraken platform");

            string publicKey = krakenKey.Public; 
            string privateKey = krakenKey.Secret;

            string signature = PlatformRequestSigner.GetKrakenSignature(
                privateKey,
                nonce,
                request.RequestUri!.AbsolutePath,
                postData
            );

            currentBody = PlatformRequestSigner.UrlEncodeData(postData);
            request.Content = new StringContent(currentBody, Encoding.UTF8, "application/x-www-form-urlencoded");

            request.Headers.Add("API-Key", publicKey);
            request.Headers.Add("API-Sign", signature);

            return await base.SendAsync(request, cancellationToken);
        }

        /// <summary>
        //  Creates new HttpContent containing content in HttpContent appended by postData. Does not check for dictionary key conflicts!
        /// </summary>
        /// <param name="postData">Data to be appended</param>
        /// <param name="content"></param>
        /// <returns>new HttpContent with appended postData</returns>
        public async Task<HttpContent> AppendJsonHttpContent(Dictionary<string,object> postData, HttpContent? content)
        {
            Dictionary<string, object> updatedDictionary;

            if (content is null) updatedDictionary = new Dictionary<string, object>();
            else 
            {
                string currentBody = await content.ReadAsStringAsync();
                updatedDictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(currentBody) ?? new Dictionary<string, object>();
            }

            updatedDictionary = updatedDictionary.Concat(postData).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            string updatedData = JsonSerializer.Serialize(updatedDictionary);

            return new StringContent(updatedData, Encoding.UTF8, "application/json");
        }
    }
}
