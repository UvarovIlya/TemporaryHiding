using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Autodesk.Revit.DB;
using System.Windows.Controls;
using System.ComponentModel;

namespace TemporaryHiding
{
    public class ParameterSearchFilter
    {
        public ParameterSearchFilter(ICollectionView filteredView, TextBox textBox)
        {
            string filterName = "";

            filteredView.Filter = delegate (object obj)
            {
                if (String.IsNullOrEmpty(filterName))
                    return true;

                string str = (obj as MyParameter).Name as string;
                if (String.IsNullOrEmpty(str))
                    return false;

                int index = str.IndexOf(filterName, 0, StringComparison.InvariantCultureIgnoreCase);

                return index > -1;
            };

            textBox.TextChanged += delegate
            {
                filterName = textBox.Text;
                filteredView.Refresh();
            };
        }
    }
}
