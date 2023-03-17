namespace OneSeedApi.Model
{
    public class CommentsResult
    {
        public int Id { get; set; }
        public string? Content { get; set; }
        public DateTime CreateTime { get; set; }
        public int Like { get; set; }
        public int Dislike { get; set; }
        public int Uid { get; set; }
        public string? Uname { get; set; }
        public string? Uhead { get; set; }

    }
}
