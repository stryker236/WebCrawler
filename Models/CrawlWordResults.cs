namespace WebCrawler.Models;

public class CrawlWordResults
{
    public long Id { get; set; }

    public long CrawlId { get; set; }
    public Crawls Crawl { get; set; } = null!;

    public long CrawlUrlId { get; set; }
    public CrawlUrls CrawlUrl { get; set; } = null!;

    public string Word { get; set; } = null!;
    public int Count { get; set; }
}
