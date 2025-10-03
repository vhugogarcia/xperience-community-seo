namespace XperienceCommunity.SEO.Models;

/// <summary>
/// Implementation of Website Discovery configuration options
/// </summary>
public class WebsiteDiscoveryOptions : IWebsiteDiscoveryOptions
{
    public string ReusableSchemaName { get; set; } = string.Empty;
    public string DefaultLanguage { get; set; } = string.Empty;
    public string SitemapShowFieldName { get; set; } = string.Empty;
    public string DescriptionFieldName { get; set; } = string.Empty;
    public string TitleFieldName { get; set; } = string.Empty;
    public string[] ContentTypeDependencies { get; set; } = [];
}
