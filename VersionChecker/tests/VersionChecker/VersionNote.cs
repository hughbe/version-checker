using Xml.Net;

namespace VersionChecker
{
    [XmlConvertCustomElement("Note")]
    public class VersionNote
    {
        public VersionNote()
        {
        }

        public VersionNote(string title, string content)
        {
            Utilities.CheckStringParam(title, SR.VersionNoteTitleEmpty, nameof(title));
            Utilities.CheckStringParam(content, SR.VersionNoteContentEmpty, nameof(content));

            Title = title;
            Content = content;
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set
            {
                Utilities.CheckStringParam(value, SR.VersionNoteTitleEmpty, nameof(value));

                _title = value;
            }
        }

        private string _content;
        public string Content
        {
            get { return _content; }
            set
            {
                Utilities.CheckStringParam(value, SR.VersionNoteContentEmpty, nameof(value));
                _content = value;
            }
        }
    }
}
