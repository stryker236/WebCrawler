using System.Linq.Expressions;
using WebCrawler.Models;

namespace WebCrawler.Data.Repo;

public interface ICrawlWordResultsRepo
{
    Task CreateWordResult(CrawlWordResults wordResult);
    Task UpdateWordResult(CrawlWordResults wordResult);
    Task DeleteWordResult(long id);
    Task<IEnumerable<CrawlWordResults>> GetAllWordResults();
    Task<CrawlWordResults?> GetWordResultById(long id);
    Task<IEnumerable<CrawlWordResults>> GetWordResultsByCrawlUrlId(long crawlUrlId);
    Task<IEnumerable<CrawlWordResults>> GetWordResultsByCrawlId(long crawlId);
    Task<IEnumerable<CrawlWordResults>> GetWordResultsByWord(string word);

    
}