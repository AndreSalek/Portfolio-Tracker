using BackendLibrary;

namespace PortfolioTrackerTests
{
    public class KrakenTests
    {
        [Fact]
        public void PlatformRequestSigner_KrakenSignatureSuccess()
        {
            string privateKey = "kQH5HW/8p1uGOVjbgWA7FunAmGO8lsSUXNsu3eow76sz84Q18fWxnyRzBHCd3pd5nE9qa99HAZtuZuj6F1huXg==";
            long nonce = 1616492376594;
            string actionPath = "/0/private/AddOrder";
            Dictionary<string, string> payload = new Dictionary<string, string>()
            {
                { "nonce", "1616492376594" },
                { "ordertype", "limit" },
                { "pair", "XBTUSD" },
                { "price", "37500" },
                { "type", "buy" },
                { "volume", "1.25" }
            };
            string apiSignExpected = "4/dpxb3iT4tp/ZCVEwSnEsLxx0bqyhLpdfOpc6fn7OR8+UClSV5n9E6aSS8MPtnRfp32bAb0nmbRn6H8ndwLUQ==";

            string apiSignGenerated = PlatformRequestSigner.GetKrakenSignature(privateKey, nonce, actionPath, payload);

            Assert.Equal(apiSignExpected, apiSignGenerated);
        }
    }
}

