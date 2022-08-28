namespace VideoMeta.Data;

public class Theme
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; }

    public DateTime CreatedTime { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedTime { get; set; } = DateTime.UtcNow;

    public bool IsDeleted { get; set; }
}