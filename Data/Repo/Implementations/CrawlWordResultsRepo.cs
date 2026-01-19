using WebCrawler.Models;
using Microsoft.EntityFrameworkCore;

namespace WebCrawler.Data.Repo;

public class CrawlWordResultsRepo : ICrawlWordResultsRepo
{
    private readonly CrawlerDbContext _db;

    public CrawlWordResultsRepo(CrawlerDbContext db)
    {
        _db = db;
    }

    public async Task CreateWordResult(CrawlWordResults wordResult)
    {
        await _db.CrawlWordResults.AddAsync(wordResult);
        await _db.SaveChangesAsync();
    }

    
    public async Task UpdateWordResult(CrawlWordResults wordResult)
    {
        _db.CrawlWordResults.Update(wordResult);
        await _db.SaveChangesAsync();
    }

    

    public async Task DeleteWordResult(long id)
    {
        CrawlWordResults? wordResult = await _db.CrawlWordResults.FindAsync(id);
        if (wordResult != null)
        {
            _db.CrawlWordResults.Remove(wordResult);
            await _db.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<CrawlWordResults>> GetAllWordResults()
    {
        return await _db.CrawlWordResults.ToListAsync();
    }

    public async Task<CrawlWordResults?> GetWordResultById(long id)
    {
        return await _db.CrawlWordResults.FindAsync(id);
    }

    public async Task<IEnumerable<CrawlWordResults>> GetWordResultsByCrawlId(long crawlId)
    {
        return await _db.CrawlWordResults
            .Where(wr => wr.CrawlId == crawlId)
            .ToListAsync();
    }

    public async Task<IEnumerable<CrawlWordResults>> GetWordResultsByCrawlUrlId(long crawlUrlId)
    {
        return await _db.CrawlWordResults
            .Where(wr => wr.CrawlUrlId == crawlUrlId)
            .ToListAsync();
    }

    public async Task<IEnumerable<CrawlWordResults>> GetWordResultsByWord(string word)
    {
        return await _db.CrawlWordResults
            .Where(wr => wr.Word == word)
            .ToListAsync();
    }

}