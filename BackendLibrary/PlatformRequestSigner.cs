using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace BackendLibrary
{
    public class PlatformRequestSigner
    {
        public static string GetKrakenSignature(string privateKey, long nonce, string uriPath, Dictionary<string, string> data)
        {
            // https://docs.kraken.com/api/docs/guides/spot-rest-auth/
            // HMAC-SHA512 of (URI path + SHA256(nonce + POST data)) and base64 decoded secret API key
            byte[] decodedPrivateKey = Convert.FromBase64String(privateKey);
            string postData = string.Join("&", data.Select(i => i.Key + "=" + i.Value));

            byte[] binData = Encoding.UTF8.GetBytes(nonce + postData);
            SHA256 sha256 = SHA256.Create();
            byte[] shaHashValue = sha256.ComputeHash(binData);

            byte[] msg = Encoding.UTF8.GetBytes(uriPath).Concat(shaHashValue).ToArray();
            HMACSHA512 hmac = new HMACSHA512(decodedPrivateKey);
            byte[] result = hmac.ComputeHash(msg);

            return Convert.ToBase64String(result);
        }
    }
}
