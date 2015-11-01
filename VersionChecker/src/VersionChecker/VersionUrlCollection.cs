using System.Collections.Generic;
using System.Xml.Linq;

namespace VersionChecker
{
    public class VersionUrlCollection : VersionCustomCollection<VersionUrl>
    {
        public VersionUrlCollection() : base() { }
        public VersionUrlCollection(IList<VersionUrl> list) : base(list) { }

        public VersionUrlCollection(XElement element) : base(element)
        {
        }

        protected internal override string XmlIdentifier => "Urls";
    }
}
