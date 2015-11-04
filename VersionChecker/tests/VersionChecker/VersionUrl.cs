using Xml.Net;

namespace VersionChecker
{
    [XmlConvertCustomElement("Note")]
    public class VersionUrl
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
    }
}
