namespace WebCrawler.Models;

public class CrawlResultsTable
{
    public int Id { get; set; }
    public int crawlID { get; set; }
    public string url { get; set; } = null!;
    public string word { get; set; } = null!;
    public int count { get; set; }

}