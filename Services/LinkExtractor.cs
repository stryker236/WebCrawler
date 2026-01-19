using System.ComponentModel.DataAnnotations;
using HtmlAgilityPack;
public class LinkExtractor
{
    public IEnumerable<string> ExtractWithHtmlAgilityPackEnumerable(string html)
    {
        var doc = new HtmlDocument();
        doc.LoadHtml(html);
        var nodes = doc.DocumentNode.SelectNodes("//a[@href]");
        if (nodes == null) yield break;
        foreach (var node in nodes)
        {
            var href = node.GetAttributeValue("href", null!);
            if (Uri.IsWellFormedUriString(href, UriKind.Absolute))
            if (!string.IsNullOrWhiteSpace(href)) yield return href;
        }
    }

    public List<string> ExtractWithHtmlAgilityPackList(string html)
    {
        var links = new List<string>();
        var doc = new HtmlDocument();
        doc.LoadHtml(html);
        var nodes = doc.DocumentNode.SelectNodes("//a[@href]");
        if (nodes == null) return links;
        foreach (var node in nodes)
        {
            var href = node.GetAttributeValue("href", null!);
            if (Uri.IsWellFormedUriString(href, UriKind.Absolute))
            if (!string.IsNullOrWhiteSpace(href)) links.Add(href);
        }
        return links;
    }

    public void RegularExpressions(string html)
    {
        // TODO : Implementar extração de links usando expressões regulares
    }   
}