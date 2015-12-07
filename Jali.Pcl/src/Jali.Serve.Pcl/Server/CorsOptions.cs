using System.Collections.Generic;

namespace Jali.Serve.Server
{
    public class CorsOptions
    {
        public bool SupportsCors { get; set; }
        public IEnumerable<string> AllowedOrigins { get; set; }
        public bool AllowAllOrigins { get; set; }
        public bool SupportsCredentials { get; set; }
    }
}
