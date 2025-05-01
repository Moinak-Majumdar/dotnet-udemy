namespace user_auth.Dto
{
    public class PostUpdateDto
    {

        public long PostId { get; set; }
        public string PostTitle { get; set; } = "";
        public string PostContent { get; set; } = "";
    }
}