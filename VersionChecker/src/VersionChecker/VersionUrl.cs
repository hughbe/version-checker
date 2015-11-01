using System;
using System.Xml.Linq;

namespace VersionChecker
{
    public class VersionUrl : XmlConvertible
    {
        public VersionUrl()
        {
        }

        public VersionUrl(string title, string url)
        {
            Utilities.CheckStringParam(title, SR.VersionUrlTitleEmpty, nameof(title));
            Utilities.CheckStringParam(url, SR.VersionUrlEmpty, nameof(url));

            Title = title;
            Url = url;
        }

        protected internal override XElement ToXElement()
        {
            var element = new XElement(XmlIdentifier);

            element.Add(new XElement("Title", Title));
            element.Add(new XElement("Url", Url));

            return element;
        }

        protected internal override XmlConvertible FromXElement(XElement element)
        {
            Utilities.CheckParameter(element, nameof(element));

            var title = element.Element("Title")?.Value;
            var url = element.Element("Url")?.Value;

            if (title == null) { throw new ArgumentException(SR.VersionUrlTitleMissing); }
            if (url == null) { throw new ArgumentException(SR.VersionUrlMissing); }

            return new VersionUrl(title, url);
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set
            {
                Utilities.CheckStringParam(value, SR.VersionUrlTitleEmpty, nameof(value));

                _title = value;
            }
        }

        private string _url;
        public string Url
        {
            get { return _url; }
            set
            {
                Utilities.CheckStringParam(value, SR.VersionUrlEmpty, nameof(value));
                _url = value;
            }
        }

        protected internal override string XmlIdentifier => "Url";
    }
}
