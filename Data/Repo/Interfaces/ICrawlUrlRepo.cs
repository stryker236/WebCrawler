using WebCrawler.Models;

namespace WebCrawler.Data.Repo;

public interface ICrawlUrlRepo
{
    Task CreateUrl(CrawlUrls url);
    Task UpdateUrl(CrawlUrls url);
    Task<IEnumerable<CrawlUrls>> GetAllUrls();
    Task<CrawlUrls?> GetUrlById(long id);
    Task DeleteUrl(long id);
}