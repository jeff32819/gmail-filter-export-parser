using GmailFilterExportParser;
using Newtonsoft.Json;

namespace ConsoleApp1;

internal class Program
{
    private static void Main(string[] args)
    {
        const string filePath = @"T:\mailFilters.xml";
        var xml = File.ReadAllText(filePath);
        var rawData = XmlParser.GetRawData(xml);

        var uniqueKeys = XmlParser.GetEntryItems(rawData);
        File.WriteAllText(@"T:\gmail-filters-entry-items.json.txt", JsonConvert.SerializeObject(uniqueKeys, Formatting.Indented));


        var entries = XmlParser.GetUniqueEntryKeyNames(rawData);
        File.WriteAllText(@"T:\gmail-filters-unique-keys.txt", JsonConvert.SerializeObject(entries, Formatting.Indented));

        Console.ReadLine();
    }
}