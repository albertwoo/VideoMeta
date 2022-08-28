namespace VideoMeta.Functions.Dtos;

public record VideoMetaEvent(
    Guid Id,
    VideoMetaEventType Type,
    Guid? ThemeId,
    string? Url,
    string? Title,
    string? Description
);

public enum VideoMetaEventType
{
    CREATED,
    UPDATED,
    DELETED,
    RECOVER,
    PROCESSED
}
