using Microsoft.EntityFrameworkCore;

namespace ResearchAtlas.Infrastructure.Persistence.SqlServer.Context
{
    public sealed class ResearchAtlasDbContext : DbContext
    {
        public ResearchAtlasDbContext(
            DbContextOptions<ResearchAtlasDbContext> options)
            : base(options)
        {
        }        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(ResearchAtlasDbContext).Assembly);
        }
    }
}
