using System.Text.RegularExpressions;

namespace StarWarsInfo.Integrations.Swapi;

public static class SwapiFieldParser
{
    /// <summary>
    /// Checks if the text is null, empty, whitespace, or contains "unknown" or "n/a".
    /// This is used to determine if a value should be treated as null.
    /// </summary>
    /// <param name="text">Raw text value from a SWAPI field.</param>
    /// <returns>
    /// True=The text is null, empty, whitespace, or has value that is considered null.
    /// False=Text is not considered null.
    /// </returns>
    private static bool TextIsNull(string? text) 
    {
        var lowerText = text?.ToLower();
        return 
            string.IsNullOrWhiteSpace(lowerText) || 
            lowerText == "unknown" || 
            lowerText == "indefinite" ||
            lowerText == "n/a";
    }
    
    /// <summary>
    /// Removes all non-numeric characters from a string, except for the decimal point.
    /// This is useful for cleaning up raw text input that should represent a number, but
    /// may contain extraneous characters such as currency symbols, commas, or other non-numerics.
    /// </summary>
    /// <param name="text">Raw numeric text value that may contain extra characters.</param>
    /// <returns>Text value containing only numerics and period.</returns>
    private static string RemoveNonNumerics(string text)
    {
        return Regex.Replace(text, "[^\\d\\.]", "");
    }
    
    /// <summary>
    /// Converts a raw text value to an integer, returning null if the text is considered null.
    /// This method is useful for converting SWAPI fields that may contain numeric values
    /// with extraneous characters or string literals that should be treated as null.
    /// </summary>
    /// <param name="rawText">Raw numeric text value that may contain extra characters.</param>
    /// <returns>Parsed integer value from the sanitized text or null.</returns>
    public static int? RawTextToIntNullable(string rawText)
    {
        if (TextIsNull(rawText)) {
            return null;
        }
        
        return int.Parse(RemoveNonNumerics(rawText));
    }
    
    /// <summary>
    /// Converts a raw text value to a decimal, returning null if the text is considered null.
    /// This method is useful for converting SWAPI fields that may contain numeric values
    /// with extraneous characters or string literals that should be treated as null.
    /// </summary>
    /// <param name="rawText">Raw numeric text value that may contain extra characters.</param>
    /// <returns>Parsed decimal value from the sanitized text or null.</returns>
    public static decimal? RawTextToDecimalNullable(string rawText)
    {
        if (TextIsNull(rawText)) {
            return null;
        }
        
        return decimal.Parse(RemoveNonNumerics(rawText));
    }
    
    /// <summary>
    /// Converts a raw text value to a decimal.
    /// This method is useful for converting SWAPI fields that may contain numeric values
    /// with extraneous characters.
    /// </summary>
    /// <param name="rawText">Raw numeric text value that may contain extra characters.</param>
    /// <returns>Parsed decimal value from the sanitized text.</returns>
    public static decimal RawTextToDecimal(string rawText)
    {
        return decimal.Parse(RemoveNonNumerics(rawText));
    }
    
    /// <summary>
    /// Converts a raw text value to an unsigned long integer, returning null if the text is considered null.
    /// This method is useful for converting SWAPI fields that may contain numeric values
    /// with extraneous characters or string literals that should be treated as null.
    /// </summary>
    /// <param name="rawText">Raw numeric text value that may contain extra characters.</param>
    /// <returns>Parsed ulong value from the sanitized text or null.</returns>
    public static ulong? RawTextToUlongNullable(string rawText)
    {
        if (TextIsNull(rawText)) {
            return null;
        }

        return ulong.Parse(RemoveNonNumerics(rawText));
    }
    
    /// <summary>
    /// Extracts the numeric identifier from the end of a URL string.
    /// This method is useful for getting the ID from SWAPI URLs that end with a numeric value.
    /// </summary>
    /// <param name="url">URL string that ends with a numeric ID (e.g., "https://swapi.info/api/starships/3").</param>
    /// <returns>The numeric ID from the end of the URL.</returns>
    public static int RawUrlToId(string url)
    {
        var parts = url.Trim().TrimEnd('/').Split('/');
        return int.Parse(parts[^1]);
    }

    
    /// <summary>
    /// Extracts the numeric identifier from the end of a URL string, returning null if the URL is null.
    /// This method is useful for getting the ID from nullable SWAPI URLs that end with a numeric value.
    /// </summary>
    /// <param name="url">URL string that ends with a numeric ID (e.g., "https://swapi.info/api/starships/3"), or null.</param>
    /// <returns>The numeric ID from the end of the URL, or null if the URL is null.</returns>
    public static int? RawUrlToIdNullable(string? url)
    {
        if (string.IsNullOrEmpty(url)) {
            return null;
        }

        return RawUrlToId(url);
    }
    
    /// <summary>
    /// Converts a raw text value to title case, where the first letter of each word is capitalized.
    /// This method is useful for formatting text fields from SWAPI that need consistent capitalization.
    /// </summary>
    /// <param name="rawText">Raw text value that needs title case formatting.</param>
    /// <returns>Text with the first letter of each word capitalized.</returns>
    public static string RawTextToTitleCase(string rawText)
    {
        if (TextIsNull(rawText)) {
            return string.Empty;
        }

        return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(rawText);
    }
}