using Microsoft.EntityFrameworkCore;
using WebCrawler.Models;

namespace WebCrawler.Data.Repo;
// NOTE: Classe que representa o contexto do banco de dados para o Entity Framework Core
public class CrawlerDbContext : DbContext
{
    public CrawlerDbContext(DbContextOptions<CrawlerDbContext> options)
        : base(options)
    { }

    // Define os DbSets para cada entidade no banco de dados, permitindo fazer operações
    public DbSet<Crawls> Crawls => Set<Crawls>();
    public DbSet<CrawlUrls> CrawlUrls => Set<CrawlUrls>();
    public DbSet<CrawlWordResults> CrawlWordResults => Set<CrawlWordResults>();
    public DbSet<CrawlConfigs> CrawlConfigs => Set<CrawlConfigs>();
    public DbSet<CrawlResultsTable> CrawlResultsTable => Set<CrawlResultsTable>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CrawlResultsTable>().HasNoKey();
    }
}
