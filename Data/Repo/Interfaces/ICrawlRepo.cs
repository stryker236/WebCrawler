using WebCrawler.Models;

namespace WebCrawler.Data.Repo;

public interface ICrawlRepo
{
    Task CreateCrawl(Crawls crawl);
    Task UpdateCrawl(Crawls crawl);
    Task<IEnumerable<Crawls>> GetAllCrawls();
    Task<Crawls?> GetCrawlById(long id);
    Task<Crawls?> GetMostRecentCrawl();

    Task<IEnumerable<CrawlResultsTable>> GetCrawlFullResults();

    Task<IEnumerable<CrawlResultsTable>> GetCrawlFullResultsByID(long id);
}