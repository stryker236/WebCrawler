using WebCrawler.Models;
using Microsoft.EntityFrameworkCore;

namespace WebCrawler.Data.Repo;

public class CrawlUrlRepo : ICrawlUrlRepo
{
    private readonly CrawlerDbContext _db;

    public CrawlUrlRepo(CrawlerDbContext db)
    {
        _db = db;
    }

    public async Task CreateUrl(CrawlUrls crawlUrl)
    {
        _db.CrawlUrls.Add(crawlUrl);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateUrl(CrawlUrls crawlUrl)
    {
        _db.CrawlUrls.Update(crawlUrl);
        await _db.SaveChangesAsync();
    }

    public async Task<IEnumerable<CrawlUrls>> GetAllUrls()
    {
        return await _db.CrawlUrls.ToListAsync();
    }

    public async Task<CrawlUrls?> GetUrlById(long id)
    {
        return await _db.CrawlUrls.FindAsync(id);
    }

    public async Task DeleteUrl(long id)
    {
        var crawlUrl = await _db.CrawlUrls.FindAsync(id);
        if (crawlUrl != null)
        {
            _db.CrawlUrls.Remove(crawlUrl);
            await _db.SaveChangesAsync();
        }
    }
}