using static GmailFilterExportParser.Models.RawData;

namespace GmailFilterExportParser.Models;

public class EntryItem(List<AppsProperty> properties)
{
    private List<AppsProperty> Properties { get; } = properties;

    /// <summary>
    ///     From email address
    /// </summary>
    public string From => GetPropertyValue("from");

    // Simplifies getting the 'subject'
    public string Subject => GetPropertyValue("subject");

    /// <summary>
    ///     Apply label
    /// </summary>
    public string Label => GetPropertyValue("label");

    /// <summary>
    ///     Forward to email address
    /// </summary>
    public string ForwardTo => GetPropertyValue("forwardTo");

    /// <summary>
    ///     Has the word in the subject or body
    /// </summary>
    public string HasTheWord => GetPropertyValue("hasTheWord");

    /// <summary>
    ///     Send to trash
    /// </summary>
    public bool ShouldTrash => IsActionTrue("shouldTrash");

    // Simplifies checking if the 'shouldMarkAsRead' action is true
    public bool ShouldMarkAsRead => IsActionTrue("shouldMarkAsRead");

    /// <summary>
    ///     Never send to spam
    /// </summary>
    public bool ShouldNeverSpam => IsActionTrue("shouldNeverSpam");

    /// <summary>
    ///     Removes email from inbox, but still exists and is searchable. If filter has a label then it will be accessible via
    ///     the label, effectively skipping the inbox.
    /// </summary>
    public bool ShouldArchive => IsActionTrue("shouldArchive");

    /// <summary>
    ///     Should always mark as important
    /// </summary>
    public bool ShouldAlwaysMarkAsImportant => IsActionTrue("shouldAlwaysMarkAsImportant");

    /// <summary>
    ///     Should add a star to the email
    /// </summary>
    public bool ShouldStar => IsActionTrue("shouldStar");

    /// <summary>
    ///     Helper method to find a specific property value
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    private string GetPropertyValue(string name)
    {
        return Properties?.FirstOrDefault(p => p.Name == name)?.Value;
    }

    /// <summary>
    ///     Helper method to check if a boolean action property is set to 'true'
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    private bool IsActionTrue(string name)
    {
        return Properties?.Any(p => p.Name == name && p.Value?.ToLowerInvariant() == "true") ?? false;
    }
}