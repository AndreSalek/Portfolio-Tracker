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

        public KrakenHttpHandler(UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;   
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;
        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            long nonce = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            Dictionary<string, string> postData = new Dictionary<string, string>();
            postData.Add("nonce", nonce.ToString());

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
                request.RequestUri.AbsolutePath,
                postData
            );

            request.Headers.Add("API-Key", publicKey);
            request.Headers.Add("API-Sign", signature);
            // Append body with required postData (nonce)
            request.Content = await AppendJsonHttpContent(postData, request.Content);

            return await base.SendAsync(request, cancellationToken);
        }

        /// <summary>
        //  Creates new HttpContent containing content in HttpContent appended by postData. Does not check for dictionary key conflicts!
        /// </summary>
        /// <param name="postData">Data to be appended</param>
        /// <param name="content"></param>
        /// <returns>new HttpContent with appended postData</returns>
        public async Task<HttpContent> AppendJsonHttpContent(Dictionary<string,string> postData, HttpContent? content)
        {
            string appendedContent = JsonSerializer.Serialize(postData);

            if (content is not null)
            {
                string currentBody = await content.ReadAsStringAsync();
                appendedContent = String.Join(appendedContent, currentBody);
            }

            return new StringContent(appendedContent);
        }
    }
}
