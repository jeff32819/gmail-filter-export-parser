using System.Xml.Linq;
using GmailFilterExportParser.Models;
using Newtonsoft.Json;

namespace GmailFilterExportParser;

public static class XmlParser
{
    /// <summary>
    ///     XML to parse
    /// </summary>
    /// <param name="xml"></param>
    public static RawData GetRawData(string xml)
    {
        var xDoc = XDocument.Parse(xml);
        var jsonText = JsonConvert.SerializeXNode(xDoc);
        var rawData = JsonConvert.DeserializeObject<RawData>(jsonText);
        if (rawData == null)
        {
            throw new Exception("Cannot parse data");
        }

        Console.WriteLine($"\nSuccessfully parsed {rawData.feed.entry.Count} Gmail filters from the XML.");
        return rawData;
    }

    public static List<EntryItem> GetEntryItems(RawData rawData)
    {
        var arr = new List<EntryItem>();
        if (rawData?.feed?.entry != null)
        {
            var uniqueValuesByName = new Dictionary<string, HashSet<string>>(StringComparer.OrdinalIgnoreCase);
            arr.AddRange(rawData.feed.entry.Select(filter => new EntryItem(filter.AppsProperty)));
        }
        else
        {
            Console.WriteLine("Error: Could not find any filter entries in the XML.");
        }
        return arr;
    }

    public static Dictionary<string, HashSet<string>> GetUniqueEntryKeyNames(RawData rawData, bool showValuesToConsole = false)
    {
        // Dictionary to collect unique values per AppsProperty.Name
        var uniqueValuesByName = new Dictionary<string, HashSet<string>>(StringComparer.OrdinalIgnoreCase);

        if (rawData?.feed?.entry != null)
        {
            Console.WriteLine("\n--- Filter Summary ---");


            // Now iterate over the strongly-typed objects
            foreach (var filter in rawData.feed.entry)
            {
                if (filter.AppsProperty != null)
                {
                    foreach (var item in filter.AppsProperty)
                    {
                        // Safely handle null name/value
                        var name = item?.Name ?? string.Empty;
                        var value = item?.Value ?? string.Empty;

                        // Add to dictionary of unique values

                        if (!uniqueValuesByName.TryGetValue(name, out var set))
                        {
                            set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                            uniqueValuesByName[name] = set;
                        }

                        if (!string.IsNullOrEmpty(value))
                        {
                            set.Add(value);
                        }

                        // Preserve existing per-item output
                        Console.WriteLine(name + " :: " + value);
                    }
                }

                Console.WriteLine("------------------------------------------");
            }

            // --- 5. Print unique values summary ---
            Console.WriteLine("\n--- Unique Values By Name ---");
            foreach (var kvp in uniqueValuesByName)
            {
                var name = kvp.Key;
                var values = kvp.Value ?? new HashSet<string>();
                Console.WriteLine($"Name: '{name}' - {values.Count} unique value(s)");
                if (!showValuesToConsole)
                {
                    continue;
                }
                Console.WriteLine();
                foreach (var v in values)
                {
                    Console.WriteLine("  - " + v);
                }
            }
        }
        else
        {
            Console.WriteLine("Error: Could not find any filter entries in the XML.");
        }

        return uniqueValuesByName;
    }
}