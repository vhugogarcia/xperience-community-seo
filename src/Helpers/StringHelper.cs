namespace XperienceCommunity.SEO.Helpers;

public static class StringHelper
{
    /// <summary>
    /// Cleans HTML tags and extra whitespace from the input string.
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static string CleanHtml(this string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return string.Empty;
        }

        // Remove HTML tags if any are still present
        string noHtml = Regex.Replace(input, "<.*?>", string.Empty);

        // Replace multiple whitespace characters with a single space
        string noExtraWhitespace = Regex.Replace(noHtml, @"\s+", " ");

        // Trim leading and trailing whitespace
        return noExtraWhitespace.Trim();
    }

    /// <summary>
    /// Escapes Markdown special characters in the input string.
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static string EscapeMarkdown(this string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return text;
        }

        // Escape all Markdown special characters using regex
        string pattern = @"([\\`*_{}[\]()#+-.!])";

        return Regex.Replace(text, pattern, "\\$1");
    }
}
