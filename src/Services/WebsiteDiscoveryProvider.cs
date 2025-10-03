using ActionResult = Microsoft.AspNetCore.Mvc.ActionResult;

namespace XperienceCommunity.SEO.Services;

public class WebsiteDiscoveryProvider(
    IProgressiveCache cache,
    IWebsiteChannelContext website,
    IContentQueryExecutor executor,
    IWebsiteDiscoveryOptions options) : IWebsiteDiscoveryProvider
{
    private readonly IProgressiveCache cache = cache;
    private readonly IWebsiteChannelContext website = website;
    private readonly IContentQueryExecutor executor = executor;
    private readonly IWebsiteDiscoveryOptions options = ValidateOptions(options);

    private static IWebsiteDiscoveryOptions ValidateOptions(IWebsiteDiscoveryOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        if (string.IsNullOrWhiteSpace(options.ReusableSchemaName))
        {
            throw new ArgumentException("ReusableSchemaName cannot be null or empty.", nameof(options));
        }

        if (string.IsNullOrWhiteSpace(options.DefaultLanguage))
        {
            throw new ArgumentException("DefaultLanguage cannot be null or empty.", nameof(options));
        }

        if (string.IsNullOrWhiteSpace(options.DescriptionFieldName))
        {
            throw new ArgumentException("DescriptionFieldName cannot be null or empty.", nameof(options));
        }

        if (string.IsNullOrWhiteSpace(options.TitleFieldName))
        {
            throw new ArgumentException("TitleFieldName cannot be null or empty.", nameof(options));
        }

        if (options.ContentTypeDependencies == null || options.ContentTypeDependencies.Length == 0)
        {
            throw new ArgumentException("ContentTypeDependencies must contain at least one content type.", nameof(options));
        }

        return options;
    }

    public async Task<ActionResult> GenerateSitemap()
    {
        var sitemapItems = await GetSitemapPages();
        return new SitemapProvider().CreateSitemap(new SitemapModel(sitemapItems));
    }
    public async Task<List<SitemapNode>> GetSitemapPages() =>
        await cache.LoadAsync(cs =>
        {
            cs.CacheDependency = CacheHelper.GetCacheDependency(BuildCacheDependencyKeys());

            return GetSitemapNodesInternal();
        }, new CacheSettings(3, [nameof(GetSitemapPages)]) { });

    private string[] BuildCacheDependencyKeys() =>
        options.ContentTypeDependencies
            .Select(t => $"webpageitem|bychannel|{website.WebsiteChannelName}|bycontenttype|{t}")
            .ToArray();

    private async Task<List<SitemapNode>> GetSitemapNodesInternal()
    {
        var nodes = new List<SitemapNode>();

        var b = new ContentItemQueryBuilder()
            .ForContentTypes(c => c
                .OfReusableSchema(options.ReusableSchemaName)
                .ForWebsite(website.WebsiteChannelName))
            .InLanguage(options.DefaultLanguage);

        var pages = await executor.GetWebPageResult(b, c =>
        {
            bool isInSitemap = string.IsNullOrWhiteSpace(options.SitemapShowFieldName)
                ? true
                : c.TryGetValue(options.SitemapShowFieldName, out bool? val) && (val ?? false);

            string description = string.Empty;
            c.TryGetValue(options.DescriptionFieldName, out description);

            string title = string.Empty;
            c.TryGetValue(options.TitleFieldName, out title);

            return new SitemapPage(new()
            {
                WebPageItemID = c.WebPageItemID,
                WebPageItemGUID = c.WebPageItemGUID,
                WebPageItemName = c.WebPageItemName,
                WebPageItemOrder = c.WebPageItemOrder,
                WebPageItemTreePath = c.WebPageItemTreePath,
                WebPageUrlPath = c.WebPageUrlPath,

                ContentItemCommonDataContentLanguageID = c.ContentItemCommonDataContentLanguageID,
                ContentItemCommonDataVersionStatus = c.ContentItemCommonDataVersionStatus,
                ContentItemContentTypeID = c.ContentItemContentTypeID,
                ContentItemGUID = c.ContentItemGUID,
                ContentItemID = c.ContentItemID,
                ContentItemIsSecured = c.ContentItemIsSecured,
                ContentItemName = c.ContentItemName,
            }, isInSitemap, title, description);
        });

        foreach (var page in pages)
        {
            if (!page.IsInSitemap)
            {
                continue;
            }

            var node = new SitemapNode(page.SystemFields.WebPageUrlPath)
            {
                LastModificationDate = DateTime.Now,
                ChangeFrequency = ChangeFrequency.Weekly,
            };

            nodes.Add(node);
        }

        return nodes;
    }

    public struct SitemapPage(WebPageFields systemFields, bool isInSitemap, string title, string description) : IWebPageFieldsSource
    {
        public WebPageFields SystemFields { get; set; } = systemFields;
        public bool IsInSitemap { get; set; } = isInSitemap;
        public string Title { get; set; } = title;
        public string Description { get; set; } = description;
    }
}
