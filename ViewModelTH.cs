using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Collections.ObjectModel;

using Prism.Commands;
using Prism.Mvvm;
using System.ComponentModel;
using System.Windows.Data;

namespace TemporaryHiding
{
    class ViewModelTH : BindableBase
    {
        private ObservableCollection<MyElement> _colSelectedElements;
        public ObservableCollection<MyElement> ColSelectedElements
        {
            get { return _colSelectedElements; }
            set
            {
                SetProperty(ref _colSelectedElements, value);
            }
        }

        private int _selectedCategoryId;
        public int SelectedCategoryId
        {
            get { return _selectedCategoryId; }
            set
            {
                SetProperty(ref _selectedCategoryId, value);
            }
        }
        //private ObservableCollection<string> _colCatNames;
        //public ObservableCollection<string> ColCatNames
        //{
        //    get { return _colCatNames; }
        //    set
        //    {
        //        SetProperty(ref _colCatNames, value);
        //    }
        //}

        public MyParameter SelectedParameter
        {
            get { return View.ListBoxParameters.SelectedItem as MyParameter; }
        }

        //public object SelectedTreeNode
        //{
        //    get { return View.TreeViewParameters.SelectedItem; }
        //}

        private ObservableCollection<MyParameter> _colParameters;
        public ObservableCollection<MyParameter> ColParameters
        {
            get { return _colParameters; }
            set
            {
                SetProperty(ref _colParameters, value);
            }
        }

        private string _searchName;
        public string SearchName
        {
            get { return _searchName; }
            set
            {
                _searchName = value;
                SelectedParameterItem = ColParameters.FirstOrDefault(x => !string.IsNullOrEmpty(x.Name) &&
                x.Name.IndexOf(_searchName, StringComparison.CurrentCultureIgnoreCase) >= 0);
                SetProperty(ref _searchName, value);                
            }
        }

        private MyParameter _selectedParameter;
        public MyParameter SelectedParameterItem
        {
            get { return _selectedParameter; }
            set
            {
                if (_selectedParameter != value)
                {
                    SetProperty(ref _selectedParameter, value);
                }
            }
        }

        private void SelectElemsAction()
        {
            //_colSelectedElements = RevitModel.Select();
        }
        public ICommand Select
        {
            get { return new DelegateCommand(SelectElemsAction); }
        }

        private void HidingAction()
        {
            bool res = RevitModel.HideOrIsolateElems(SelectedCategoryId, SelectedParameter, ColSelectedElements, true);
            View.DialogResult = res;
        }
        public ICommand Hiding
        {
            get { return new DelegateCommand(HidingAction); }
        }

        private void IsolateAction()
        {
            bool res = RevitModel.HideOrIsolateElems(SelectedCategoryId, SelectedParameter, ColSelectedElements, false);
            View.DialogResult = res;
        }
        public ICommand Isolate
        {
            get { return new DelegateCommand(IsolateAction); }
        }        

        internal ViewTH View { get; set; }
        internal ModelTH RevitModel { get; set; }
    }
}
