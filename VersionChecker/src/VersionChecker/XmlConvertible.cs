using System.Xml.Linq;

namespace VersionChecker
{
    public abstract class XmlConvertible
    {
        protected internal XmlConvertible()
        {
        }
        
        protected internal abstract string XmlIdentifier { get; }

        protected internal abstract XElement ToXElement();
        protected internal abstract XmlConvertible FromXElement(XElement element);
    }
}
