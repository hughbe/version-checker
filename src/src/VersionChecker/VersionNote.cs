using System;
using Xml.Net;

namespace VersionChecker
{
    [XmlConvertCustomElement("Note")]
    public class VersionNote : IEquatable<VersionNote>
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

        public override bool Equals(object obj)
        {
            return Equals(obj as VersionNote);
        }

        public bool Equals(VersionNote other)
        {
            if (other == null) { return false; }

            return Title == other.Title && Content == other.Content;
        }

        public override int GetHashCode()
        {
            return Title.GetHashCode() ^ Content.GetHashCode();
        }
    }
}
