namespace user_auth.models
{
    public class JWtSettings
    {
        public string TokenKey { get; set; } = "";
        public int ExpInMin;
    }
}