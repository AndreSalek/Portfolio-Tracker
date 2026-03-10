using BackendLibrary;

namespace Frontend.Data.Models
{
    public class PlatformKey
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public Platform Platform { get; set; }
        public string SecretKey { get; set; }
    }
}
