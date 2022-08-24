using Microsoft.EntityFrameworkCore;

namespace VideoMeta.Data
{
    public class VideoMetaDbContext : DbContext
    {
        public VideoMetaDbContext(DbContextOptions<VideoMetaDbContext> options) : base(options)
        {
        }

        public DbSet<Theme> Themes { get; set; }
        public DbSet<VideoMeta> VideoMetas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Theme>().ToContainer("Themes");
            modelBuilder.Entity<VideoMeta>().ToContainer("VideoMetas");
            base.OnModelCreating(modelBuilder);
        }
    }
}
