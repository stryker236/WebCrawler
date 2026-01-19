namespace WebCrawler.Models;

public class CrawlUrls
{
    public long Id { get; set; }

    public long CrawlId { get; set; }
    public Crawls Crawl { get; set; } = null!;

    public string Url { get; set; } = null!;
    public int Depth { get; set; }

    public DateTime? VisitedAt { get; set; }

    public ICollection<CrawlWordResults> WordResults { get; set; } = new List<CrawlWordResults>();
}
