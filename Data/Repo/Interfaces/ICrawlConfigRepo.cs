using WebCrawler.Models;

namespace WebCrawler.Data.Repo;

public interface ICrawlConfigRepo
{
    Task<CrawlConfigs?> GetConfig();
    Task CreateNewConfig(CrawlConfigs config);
}