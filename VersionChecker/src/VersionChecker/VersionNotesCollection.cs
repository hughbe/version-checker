using System.Collections.Generic;
using System.Xml.Linq;

namespace VersionChecker
{
    public class VersionNotesCollection : VersionCustomCollection<VersionNote>
    {
        public VersionNotesCollection() : base() { }
        public VersionNotesCollection(IList<VersionNote> list) : base(list) { }

        public VersionNotesCollection(XElement element) : base(element)
        {
        }

        protected internal override string XmlIdentifier => "Notes";
    }
}
