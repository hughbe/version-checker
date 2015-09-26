using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

namespace Versions
{
    public class Version
    {
        public Version(string id, DateTime date, List<string> notes, string url)
        {
            Id = StandardizedString(id);
            Notes = notes ?? new List<string>();
            Date = date;
            Url = url ?? "";
        }

        public static Version CurrentVersion => new Version(Assembly.GetExecutingAssembly().GetName().Version.ToString(), DateTime.Now, null, null);

        public string Id { get; }
        public DateTime Date { get; }

        public List<string> Notes { get; }

        public string Url { get; }

        private string StandardizedString(string original)
        {
            var replaced = Regex.Replace(original, @"\r\n?|\n", "").Replace(" ", "");
            return replaced;
        }

        public string ToJson()
        {
            var dateString = Date.Day + "/" + Date.Month + "/" + Date.Year + " " + Date.Hour + ":" + Date.Minute + ":" + Date.Second;
            var xmlDoc = new XDocument(
                            new XElement("Version",
                                new XElement("Id", Id),
                                new XElement("Date", dateString),
                                new XElement("Notes",
                                    new XElement("Note", Notes.ToArray())),
                                new XElement("Url", Url)));
            return xmlDoc.ToString();
        }

        public static Version FromJson(string xml)
        {
            try
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xml);

                var info = xmlDocument.SelectSingleNode("Version");

                var id = info.SelectSingleNode("Id").InnerText;

                var date = DateTime.Now;

                var rawDate = info.SelectSingleNode("Date")?.InnerText;
                if (rawDate != null)
                {
                    date = Convert.ToDateTime(rawDate);
                }

                var notes = new List<string>();

                var notesNodes = info.SelectNodes("Notes/Note");

                if (notesNodes != null)
                {
                    foreach (XmlNode node in notesNodes)
                    {
                        notes.Add(node.InnerText);
                    }
                }

                var url = info.SelectSingleNode("Url")?.InnerText;

                return new Version(id, date, notes, url);
            }
            catch
            {
                return CurrentVersion;
            }
        }
    }

    public class VersionChecker
    {
        public VersionChecker(string versionDirectory)
        {
            if (!versionDirectory.EndsWith("/"))
            {
                versionDirectory += "/";
            }
            VersionDirectory = versionDirectory;
            UpdateLatestVersion();
        }

        public string VersionDirectory { get; }

        public Version LatestVersion { get; private set; }
        public void UpdateLatestVersion() => LatestVersion = GetVersion("currentversion");

        public Version GetVersion(string versionId)
        {
            try
            {
                var versionInfo = new WebClient().DownloadString(VersionDirectory + versionId + ".xml");
                return Version.FromJson(versionInfo);
            }
            catch
            {
                return Version.CurrentVersion;
            }
        }

        public bool UpToDate => Version.CurrentVersion.Id.Equals(LatestVersion.Id);
    }
}
