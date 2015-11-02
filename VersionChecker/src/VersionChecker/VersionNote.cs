using System;
using System.Xml.Linq;

namespace VersionChecker
{
    public class VersionNote : XmlConvertible
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

        protected internal override XElement ToXElement()
        {
            var element = new XElement(XmlIdentifier);

            element.Add(new XElement("Title", Title));
            element.Add(new XElement("Content", Content));

            return element;
        }

        protected internal override XmlConvertible FromXElement(XElement element)
        {
            Utilities.CheckParameter(element, nameof(element));

            var title = element.Element("Title")?.Value;
            var content = element.Element("Content")?.Value;

            if (title == null) { throw new ArgumentException(SR.VersionNoteTitleMissing); }
            if (content == null) { throw new ArgumentException(SR.VersionNoteContentMissing); }

            return new VersionNote(title, content);
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

        protected internal override string XmlIdentifier => "Note";
    }
}
