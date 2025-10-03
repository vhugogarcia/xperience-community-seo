using System.Text.RegularExpressions;

namespace XperienceCommunity.SEO.Helpers;

public static class StringHelper
{
    /// <summary>
    /// Truncates a string to the specified maximum length and appends a truncation suffix if necessary.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="maxLength"></param>
    /// <param name="truncationSuffix"></param>
    /// <returns></returns>
    public static string? Truncate(this string? value, int maxLength, string truncationSuffix = "…")
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return "";
        }

        return value?.Length > maxLength
             ? value[..maxLength] + truncationSuffix
             : value;
    }

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
    /// Replaces any special character (non-alphanumeric), whitespace, and underscores with hyphens.
    /// Multiple consecutive special characters are replaced with a single hyphen.
    /// Leading and trailing hyphens are removed.
    /// </summary>
    /// <param name="input">The input string to process</param>
    /// <returns>A string with special characters replaced by hyphens</returns>
    public static string ReplaceSpecialChars(this string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return input;
        }

        // Replace any non-alphanumeric character (including whitespace and _) with hyphen
        string result = Regex.Replace(input, @"[^a-zA-Z0-9]", "-");

        return result;
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
