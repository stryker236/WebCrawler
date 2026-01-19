using WebCrawler.Models;
using Microsoft.EntityFrameworkCore;
namespace WebCrawler.Data.Repo;

public class CrawlRepo : ICrawlRepo
{
    private readonly CrawlerDbContext _db;

    public CrawlRepo(CrawlerDbContext db)
    {
        _db = db;
    }

    public async Task CreateCrawl(Crawls crawl)
    {
        await _db.Crawls.AddAsync(crawl);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateCrawl(Crawls crawl)
    {
        _db.Crawls.Update(crawl);
        await _db.SaveChangesAsync();
    }

    public async Task<IEnumerable<Crawls>> GetAllCrawls()
    {
        return await _db.Crawls.ToListAsync();
    }

    public async Task<Crawls?> GetCrawlById(long id)
    {
        return await _db.Crawls.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Crawls?> GetMostRecentCrawl()
    {
        return await _db.Crawls.OrderByDescending(c => c.Id).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<dynamic>> GetCrawlTableData()
    {
        return await _db.Crawls
            .FromSqlRaw(@"
                SELECT c.id as crawl_ID,cu.Url, cwr.Word , cwr.Count FROM Crawls c
                    INNER JOIN CrawlUrls cu ON c.id = cu.CrawlId 
                    INNER JOIN CrawlWordResults cwr ON c.id = cwr.Id;
            ")
            .ToListAsync<dynamic>();
    }

    public async Task<IEnumerable<CrawlResultsTable>> GetCrawlFullResults()
    {
    return await _db.CrawlResultsTable.FromSqlRaw(@"
        SELECT
            cwr.Id        AS Id,
            c.Id          AS CrawlId,
            cu.Url        AS Url,
            cwr.Word      AS Word,
            cwr.Count     AS Count
        FROM CrawlWordResults cwr
        INNER JOIN CrawlUrls cu ON cu.Id = cwr.CrawlUrlId
        INNER JOIN Crawls c ON c.Id = cwr.CrawlId
    ").ToListAsync();
    }

    public async Task<IEnumerable<CrawlResultsTable>> GetCrawlFullResultsByID(long id)
    {
        return await _db.CrawlResultsTable.FromSqlRaw($@"
            SELECT
                cwr.Id        AS Id,
                c.Id          AS CrawlId,
                cu.Url        AS Url,
                cwr.Word      AS Word,
                cwr.Count     AS Count
            FROM CrawlWordResults cwr
            INNER JOIN CrawlUrls cu ON cu.Id = cwr.CrawlUrlId
            INNER JOIN Crawls c ON c.Id = cwr.CrawlId
            WHERE c.id = {id};
        ").ToListAsync();
    }
}