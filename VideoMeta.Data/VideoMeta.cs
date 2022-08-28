namespace VideoMeta.Data
{
    public class VideoMeta
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid ThemeId { get; set; }

        public string Url { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public bool IsReady { get; set; }


        public DateTime CreatedTime { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedTime { get; set; } = DateTime.UtcNow;

        public bool IsDeleted { get; set; }
    }
}
