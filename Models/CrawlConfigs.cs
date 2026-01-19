namespace WebCrawler.Models;

public class CrawlConfigs
{
    public int Id { get; set; }
    public string startUrls { get; set; } = null!;
    public string keywords { get; set; } = null!;
    public int maxDepth { get; set; }
    public int maxChildLinks { get; set; }
    public int crawlPeriod { get; set; }
}