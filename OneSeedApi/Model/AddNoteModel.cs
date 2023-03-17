namespace OneSeedApi.Model
{
    public class AddNoteModel
    {
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string[] Tags { get; set; } = Array.Empty<string>();
    }
}
