using Frontend.Common;
using PortfolioTracker.Core.Models.Common;

namespace Frontend.Data.Models
{
    public class PlatformKey
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public Platform Platform { get; set; }
        public string Secret { get; set; }
        public string Public { get; set; }
    }
}
