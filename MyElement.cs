using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace TemporaryHiding
{
    class MyElement
    {
        private Element _element;

        public MyElement(Element element)
        {
            _element = element;
        }

        public Element Element
        {
            get { return _element; }
        }
    }
}
