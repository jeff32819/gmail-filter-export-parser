using Newtonsoft.Json;
namespace GmailFilterExportParser.Models
{
    public class RawData
    {
        // The Gmail filter XML structure uses XML elements with attributes
        // like <apps:property name='from' value='...'/>.
        // When Newtonsoft.Json converts this to JSON, it handles the attributes
        // by prefixing them with '@', and the nested element becomes an array property.

        // --- 1. C# Class Definitions ---

        /// <summary>
        /// Represents a single <apps:property> element in the XML, which contains the filter criteria (name) and its value.
        /// </summary>
        public class AppsProperty
        {
            // These properties map to the XML attributes 'name' and 'value', which become '@name' and '@value' in JSON.
            [JsonProperty("@name")]
            public string Name { get; set; }

            [JsonProperty("@value")]
            public string Value { get; set; }
        }

        /// <summary>
        /// Represents a single <entry> element, which is one filter rule.
        /// </summary>
        public class FilterEntry
        {
            // The <atom:title> in the XML, which defaults to "Mail Filter".
            public string title { get; set; }

            // This is the core collection: an array of all criteria and actions for this filter.
            [JsonProperty("apps:property")]
            public List<AppsProperty> AppsProperty { get; set; }


        }

        /// <summary>
        /// Represents the <atom:feed> element, which contains the list of all filters.
        /// </summary>
        public class Feed
        {
            // The collection of all filter entries
            public List<FilterEntry> entry { get; set; }

            [JsonProperty("atom:title")]
            public string AtomTitle { get; set; }
        }

        /// <summary>
        /// The root object for the JSON structure after XML conversion.
        /// </summary>
        public Feed feed { get; set; }
    }
}
