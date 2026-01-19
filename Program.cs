
using Microsoft.EntityFrameworkCore;
using WebCrawler.Data.Repo;
using WebCrawler.Models;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Default"); // NOTE: Pega a connection string do appsettings.json
builder.Services.AddDbContext<CrawlerDbContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
builder.Services.AddScoped<ICrawlRepo, CrawlRepo>();
builder.Services.AddScoped<ICrawlUrlRepo, CrawlUrlRepo>();
builder.Services.AddScoped<ICrawlWordResultsRepo, CrawlWordResultsRepo>();
builder.Services.AddScoped<ICrawlConfigRepo, CrawlConfigRepo>();
builder.Services.AddHttpClient<Fetcher>();
builder.Services.AddSingleton<LinkExtractor>();
builder.Services.AddSingleton<WordCounter>();
builder.Logging.AddFilter("Microsoft.EntityFrameworkCore", LogLevel.Warning);

var app = builder.Build();

app.MapGet("/config/get", async (
    ICrawlConfigRepo configRepo) =>
{
    return await configRepo.GetConfig();
});

app.MapPost("/config/set", async (
    CrawlConfigs config,
    ICrawlConfigRepo repo) =>
{
    await repo.CreateNewConfig(config);
    return "Crawl configuration saved.";
});

app.MapGet("/crawl/start", async (
    ICrawlRepo repo,
    ICrawlUrlRepo urlRepo,
    ICrawlWordResultsRepo wordResultsRepo,
    ICrawlConfigRepo configRepo) =>
{
    var configs = await configRepo.GetConfig();

    // TODO: Check is I can improve the feedback messages
    if (configs == null) { return "No crawl configuration found."; }
    if (string.IsNullOrWhiteSpace(configs.startUrls)) { return "No start URLs defined in the configuration."; }
    if (configs.maxDepth <= 0) { return "Max depth must be greater than zero."; }
    if (string.IsNullOrWhiteSpace(configs.keywords)) { return "No keywords defined in the configuration."; }

    var crawl = new Crawls
    {
        StartUrl = configs.startUrls,
        Keyword = configs.keywords,
        MaxDepth = configs.maxDepth,
        Status = "Pending",
        StartedAt = DateTime.UtcNow
    };

    await repo.CreateCrawl(crawl);

    Queue<(string url, int depth)> crawlQueue = new Queue<(string url, int depth)>();
    List<string> visitedUrls = new List<string>();
    crawlQueue.Enqueue((configs.startUrls, 0));

    var linkExtractor = new LinkExtractor();
    var fetcher = new Fetcher(new HttpClient());
    var wordCounter = new WordCounter();

    while (crawlQueue.Count > 0)
    {
        Console.WriteLine($"Queue size: {crawlQueue.Count}");
        var (currentUrls, currentDepth) = crawlQueue.Dequeue();
        List<string> linksToProcess = currentUrls.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();
        List<string> keywordsToProcess = configs.keywords.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();

        Console.WriteLine($"Processing URL: {string.Join(", ", linksToProcess)} at depth {currentDepth}");
        
        foreach (var urlProcessing in linksToProcess)
        {
            foreach (var keyword in keywordsToProcess)
            {
                Console.WriteLine($"   With keyword: {keyword}");
                // if (crawlQueue.Any(elem => elem.url == currentUrls)) { continue; } // Linha duvidosa. Talvez ter de colocar este numa estrura à parte para ser verificado
                if (visitedUrls.Contains(urlProcessing)) { continue; }
                visitedUrls.Add(urlProcessing);

                var html = await fetcher.FetchPageAsync(urlProcessing);
                var count = wordCounter.CountSpecificWord(html, keyword);
                var childLinks = linkExtractor.ExtractWithHtmlAgilityPackEnumerable(html);

                var crawlUrl = new CrawlUrls
                {
                    CrawlId = crawl.Id,
                    Url = urlProcessing,
                    Depth = currentDepth,
                    VisitedAt = DateTime.UtcNow
                };
                await urlRepo.CreateUrl(crawlUrl);

                var crawlWordResult = new CrawlWordResults
                {
                    CrawlId = crawl.Id,
                    CrawlUrlId = crawlUrl.Id,
                    Word = keyword,
                    Count = count
                };
                await wordResultsRepo.CreateWordResult(crawlWordResult);

                Console.WriteLine($"Crawled: {urlProcessing} at depth {currentDepth}, found {childLinks.Count()} links, word count: {count}");

                if (configs.maxDepth == currentDepth) { continue; }
                foreach (var link in childLinks.Take(configs.maxChildLinks))
                {
                    crawlQueue.Enqueue((link, currentDepth + 1));

                    Console.WriteLine($"   Found: [{link}] at depth {currentDepth + 1}");
                }
            }
        }

    }

    return "Crawler finished successfully.";
});

app.MapGet("/crawl/getRecent", async (
    ICrawlRepo repo) =>
{
    var recentCrawls = await repo.GetMostRecentCrawl();
    Console.WriteLine("Fetched Recent Crawl:", recentCrawls);
    return recentCrawls;
});

app.MapGet("/crawl/urls/{id:long}", async (
    long id,
    ICrawlWordResultsRepo wordResultsRepo) =>
{
    Console.WriteLine("Fetching Word Results for Crawl ID:", id);
    var results = await wordResultsRepo.GetWordResultsByCrawlId(id);
    return results;
});

app.MapGet("/crawl/wordresults/{id:long}", async (
    long id,
    ICrawlWordResultsRepo wordResultsRepo) =>
{
    var result = await wordResultsRepo.GetWordResultById(id);
    return result;
});

app.MapGet("/crawl/wordCount/getAll", async (
    ICrawlWordResultsRepo wordResultsRepo) =>
{
    var wordCount = await wordResultsRepo.GetAllWordResults();
    return wordCount;
});

app.MapGet("/crawl/results", async (
    ICrawlRepo repo) =>
{
    return await repo.GetCrawlFullResults();
});
app.MapGet("/crawl/results/{id:long}", async (
    long id,
    ICrawlRepo repo) =>
{
    return await repo.GetCrawlFullResultsByID(id);
});


app.UseDefaultFiles();
app.UseStaticFiles();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CrawlerDbContext>();
    db.Database.OpenConnection();
    Console.WriteLine("DB CONNECTED");
    db.Database.CloseConnection();
}

app.Run();
