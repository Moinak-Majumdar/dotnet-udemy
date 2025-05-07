namespace user_auth.models
{
    public class DbResponse
    {
        public int Id { get; set; }
        public int StatusCode { get; set; }
        public string Response { get; set; } = "";
    }
}