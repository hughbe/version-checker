using System;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace VersionChecker
{
    public class ApplicationVersion : IEquatable<ApplicationVersion>
    {
        public ApplicationVersion(string id) : this(id, DateTime.MinValue, null, null)
        {
        }

        public ApplicationVersion(string id, DateTime date, VersionNotesCollection notes, VersionUrlCollection urls)
        {
            Utilities.CheckStringParam(id, "The version id cannot be empty", nameof(id));

            Id = StandardizedString(id);
            Date = date;
            Notes = notes;
            Urls = urls;
        }
        
        public string Id { get; set; }
        public DateTime Date { get; set; }

        public VersionNotesCollection Notes { get; set; }
        public VersionUrlCollection Urls { get; set; }

        private static string StandardizedString(string original)
        {
            var replaced = Regex.Replace(original, @"\r\n?|\n", "").Replace(" ", "");
            return replaced;
        }

        private XDocument ToXDocument()
        {
            var document = new XDocument();

            var container = new XElement("Version");
            document.Add(container);

            container.Add(new XElement("Id", Id));

            if (Date != DateTime.MinValue)
            {
                var dateString = Date.Day + "/" + Date.Month + "/" + Date.Year + " " + Date.Hour + ":" + Date.Minute + ":" + Date.Second;
                container.Add(new XElement("Date", dateString));
            }

            SerializeCollection(Notes, container);
            SerializeCollection(Urls, container);
            
            return document;
        }

        public string ToXml() => ToXDocument().ToString();

        private static void SerializeCollection<T>(VersionCustomCollection<T> collection, XElement container) where T : XmlConvertible, new()
        {
            if (collection != null && collection.Count > 0)
            {
                container.Add(collection.ToXElement());
            }
        }

        public static ApplicationVersion FromXml(string xml)
        {
            Utilities.CheckStringParam(xml, "The xml to parse cannot be empty", nameof(xml));
            
            var document = XDocument.Parse(xml);

            var info = document.Element("Version");
            if (info == null) { return null; }

            var idElement = info.Element("Id");
            if (idElement == null) { return null; }

            var id = idElement.Value;

            var date = DateTime.MinValue;

            var dateString = info.Element("Date")?.Value;
            if (dateString != null)
            {
                date = Convert.ToDateTime(dateString);
            }

            var versionNotes = new VersionNotesCollection(info);
            var versionUrls = new VersionUrlCollection(info);
            
            return new ApplicationVersion(id, date, versionNotes, versionUrls);
        }

        public bool Equals(ApplicationVersion other)
        {
            if (other == null) { return false; }

            var myId = Id;
            var otherId = other.Id;

            return myId.Equals(otherId);
        }
    }
}
