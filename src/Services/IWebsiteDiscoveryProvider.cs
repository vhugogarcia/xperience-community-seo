namespace XperienceCommunity.SEO.Services;

public interface IWebsiteDiscoveryProvider
{
    public Task<List<SitemapNode>> GetSitemapPages();
    public Task<ActionResult> GenerateSitemap();
    public Task<ActionResult> GenerateLlmsTxt();
}
