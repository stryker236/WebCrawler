using WebCrawler.Models;
using Microsoft.EntityFrameworkCore;
namespace WebCrawler.Data.Repo;

public class CrawlConfigRepo : ICrawlConfigRepo
{
    private readonly CrawlerDbContext _db;

    public CrawlConfigRepo(CrawlerDbContext db)
    {
        _db = db;
    }

    public async Task<CrawlConfigs?> GetConfig()
    {
        return await _db.CrawlConfigs
            .OrderByDescending(c => c.Id)
            .LastOrDefaultAsync();
    }

    public async Task CreateNewConfig(CrawlConfigs config)
    {
        var oldConfig = await GetConfig();
        if (oldConfig == null)
        {
            _db.CrawlConfigs.Add(config);
        }
        else
        {
            oldConfig.startUrls = config.startUrls;
            oldConfig.keywords = config.keywords;
            oldConfig.maxDepth = config.maxDepth;
            oldConfig.maxChildLinks = config.maxChildLinks;
            oldConfig.crawlPeriod = config.crawlPeriod;

            _db.CrawlConfigs.Update(oldConfig);
        }
            await _db.SaveChangesAsync();
    }
}