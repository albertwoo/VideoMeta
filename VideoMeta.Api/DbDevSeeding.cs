namespace VideoMeta.Api;

using VideoMeta.Data;


public static class DbDevSeeding
{
    public static void StartDbSeeding(this WebApplication app)
    {
        var serviceProvider = app.Services.GetService<IServiceScopeFactory>().CreateScope().ServiceProvider;
        var db = serviceProvider.GetService<VideoMetaDbContext>();
        db.Database.EnsureCreated();

        if (db.Themes.Count() == 0)
        {
            var theme = new Theme
            {
                Name = "Friends"
            };
            db.Themes.Add(theme);

            db.VideoMetas.Add(new VideoMeta
            {
                ThemeId = theme.Id,
                Title = "Best memory with friends"
            });

            db.SaveChanges();
        }
    }
}
