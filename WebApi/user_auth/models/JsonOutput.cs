using Newtonsoft.Json.Linq;

namespace user_auth.models
{
    public class JsonOutput
    {
        public int Id { get; set; }
        public int StatusCode { get; set; }
        public string Response { get; set; } = "";
    }
}