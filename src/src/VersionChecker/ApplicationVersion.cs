﻿using System;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using Xml.Net;

namespace VersionChecker
{
    [XmlConvertCustomElement("Version")]
    public class ApplicationVersion : IEquatable<ApplicationVersion>
    {
        public ApplicationVersion()
        {
        }

        public ApplicationVersion(string id) : this(id, null, null,DateTime.MinValue, null, null, null)
        {
        }

        public ApplicationVersion(string id, string shortDescription, string longDescription, DateTime date, Collection<VersionNote> notes, Collection<VersionUrl> urls, string copyright)
        {
            Utilities.CheckStringParam(id, "The version id cannot be empty", nameof(id));

            Id = StandardizedString(id);

            ShortDescription = shortDescription;
            LongDescription = longDescription;

            Date = date;

            Notes = notes;
            Urls = urls;

            Copyright = copyright;
        }
        
        public string Id { get; set; }

        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }

        public DateTime Date { get; set; }

        public Collection<VersionNote> Notes { get; set; }
        public Collection<VersionUrl> Urls { get; set; }

        public string Copyright { get; set; }

        private static string StandardizedString(string original)
        {
            var replaced = Regex.Replace(original, @"\r\n?|\n", "").Replace(" ", "");
            return replaced;
        }

        public string ToXml() => XmlConvert.SerializeObject(this);

        public static ApplicationVersion FromXml(string xml) => XmlConvert.DeserializeObject<ApplicationVersion>(xml);

        public bool Equals(ApplicationVersion other)
        {
            if (other == null) { return false; }

            var myId = Id;
            var otherId = other.Id;

            return myId.Equals(otherId);
        }
    }
}