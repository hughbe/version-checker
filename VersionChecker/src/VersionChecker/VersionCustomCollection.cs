using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Linq;

namespace VersionChecker
{
    public abstract class VersionCustomCollection<T> : Collection<T> where T : XmlConvertible, new()
    {
        protected VersionCustomCollection() : base() { }
        protected VersionCustomCollection(IList<T> list) : base(list) { }

        internal VersionCustomCollection(XElement parent)
        {
            Utilities.CheckParameter(parent, nameof(parent));

            var container = parent.Element(XmlIdentifier);
            if (container == null) { return; }

            var mock = new T();
            var objectIdentifier = mock.XmlIdentifier;

            var elements = container.Elements(objectIdentifier);
            if (elements == null) { return; }
            
            foreach (var element in elements)
            {
                var obj = mock.FromXElement(element);
                if (obj != null)
                {
                    Add((T)obj);
                }
            }
        }

        internal XElement ToXElement()
        {
            var element = new XElement(XmlIdentifier);

            foreach (var obj in this)
            {
                element.Add(obj.ToXElement());
            }

            return element;
        }

        protected internal abstract string XmlIdentifier { get; }
    }
}
