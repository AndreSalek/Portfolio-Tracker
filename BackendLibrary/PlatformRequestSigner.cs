using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace BackendLibrary
{
    public class PlatformRequestSigner
    {
        public static string GetKrakenSignature(string privateKey, long nonce, string uriPath, Dictionary<string, object> data)
        {
            // https://docs.kraken.com/api/docs/guides/spot-rest-auth/
            // HMAC-SHA512 of (URI path + SHA256(nonce + POST data)) and base64 decoded secret API key
            byte[] decodedPrivateKey = Convert.FromBase64String(privateKey);
            string postData = UrlEncodeData(data);

            byte[] binData = Encoding.UTF8.GetBytes(nonce + postData);
            SHA256 sha256 = SHA256.Create();
            byte[] shaHashValue = sha256.ComputeHash(binData);

            byte[] msg = Encoding.UTF8.GetBytes(uriPath).Concat(shaHashValue).ToArray();
            HMACSHA512 hmac = new HMACSHA512(decodedPrivateKey);
            byte[] result = hmac.ComputeHash(msg);

            return Convert.ToBase64String(result);
        }

        public static string UrlEncodeData(Dictionary<string, object> data)
            => string.Join("&", data.Select(i => $"{HttpUtility.UrlEncode(i.Key)}={i.Value}"));

        public static Dictionary<string, object> UrlDecodeData(string data)
        {
            string[] keyValuePairs = data.Split("&");
            Dictionary<string, object> decodedData = new Dictionary<string, object>();
            keyValuePairs.Select(pair => pair.Split("=")).ToDictionary(kvp => HttpUtility.UrlDecode(kvp[0]), kvp => kvp[1]);
            return decodedData;
        }
    }
}
