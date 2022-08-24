namespace VideoMeta.Data
{
    public class Theme
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }

        public DateTime CreatedTime { get; set; } = DateTime.Now;
        public DateTime UpdatedTime { get; set; } = DateTime.Now;

        public bool IsDeleted { get; set; }
    }
}