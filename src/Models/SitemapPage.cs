namespace XperienceCommunity.SEO.Models;

public struct SitemapPage(WebPageFields systemFields, bool isInSitemap, string title, string description) : IWebPageFieldsSource
{
    public WebPageFields SystemFields { get; set; } = systemFields;
    public bool IsInSitemap { get; set; } = isInSitemap;
    public string Title { get; set; } = title;
    public string Description { get; set; } = description;
}
