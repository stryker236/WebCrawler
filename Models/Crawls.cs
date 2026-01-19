namespace WebCrawler.Models;

public enum CrawlStatus
{
    Pending,
    Running,
    Finished,
    Failed
}

public class Crawls
{
    public long Id { get; set; }

    public string StartUrl { get; set; } = null!;
    public string Keyword { get; set; } = null!;
    public int MaxDepth { get; set; }

    public string Status { get; set; } = null!;

    public DateTime? StartedAt { get; set; }
    public DateTime? FinishedAt { get; set; }
    public int? DurationSeconds { get; set; }

    public ICollection<CrawlUrls> Urls { get; set; } = new List<CrawlUrls>();
    public ICollection<CrawlWordResults> WordResults { get; set; } = new List<CrawlWordResults>();
}
    