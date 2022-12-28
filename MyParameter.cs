using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Prism.Mvvm;

namespace TemporaryHiding
{
    class MyParameter : BindableBase
    {
        private Parameter _myParameter;
        private string _name;

        public MyParameter(Parameter parameter)
        {
            _myParameter = parameter;
            _name = parameter.Definition.Name;
        }

        public Parameter Parameter
        {
            get { return _myParameter; }
        }
        public string Name
        {
            get { return _name; }
        }
    }
}
