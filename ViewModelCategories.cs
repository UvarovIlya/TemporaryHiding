using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

using Prism.Commands;
using Prism.Mvvm;

namespace TemporaryHiding
{
    class ViewModelCategories : BindableBase
    {
        private ObservableCollection<MyParameter> _colParams;
        public ObservableCollection<MyParameter> ColParams
        {
            get { return _colParams; }
            set
            {
                SetProperty(ref _colParams, value);
            }
        }        

        private string _category;
        public string Category
        {
            get { return _category; }
            set
            {
                SetProperty(ref _category, value);
            }
        }
    }
}
