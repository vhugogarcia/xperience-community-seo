namespace XperienceCommunity.SEO.Models;

/// <summary>
/// Configuration options for WebsiteDiscovery generation
/// </summary>
public interface IWebsiteDiscoveryOptions
{
    /// <summary>
    /// The reusable schema name to filter content items
    /// </summary>
    public string ReusableSchemaName { get; }

    /// <summary>
    /// The default language for sitemap content
    /// </summary>
    public string DefaultLanguage { get; }

    /// <summary>
    /// Field name for sitemap visibility flag (optional - if empty, all pages are included)
    /// </summary>
    public string SitemapShowFieldName { get; }

    /// <summary>
    /// Field name for description
    /// </summary>
    public string DescriptionFieldName { get; }

    /// <summary>
    /// Field name for title
    /// </summary>
    public string TitleFieldName { get; }

    /// <summary>
    /// Content type names to include in sitemap
    /// </summary>
    public string[] ContentTypeDependencies { get; }
}
