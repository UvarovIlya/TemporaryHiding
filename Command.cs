using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TemporaryHiding
{
    [Transaction(TransactionMode.Manual)]
    class Command : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;

            try
            {
                Selection sel = uidoc.Selection;

                ObservableCollection<MyElement> colSelElems = new ObservableCollection<MyElement>();
                //ObservableCollection<string> colCatNames = new ObservableCollection<string>();
                //ObservableCollection<ViewModelCategories> colVMChilds = new ObservableCollection<ViewModelCategories>();
                ObservableCollection<MyParameter> colParameters = new ObservableCollection<MyParameter>();
                ICollection<ElementId> selCollectionId = sel.GetElementIds();
                                
                if (selCollectionId.Count == 0)
                {
                    IList<Reference> picked = null;
                    picked = sel.PickObjects(ObjectType.Element, "Выберите элементы одной категории");
                    
                    foreach (Reference refer in picked)
                    {
                        var elem = doc.GetElement(refer);
                        
                        selCollectionId.Add(elem.Id);
                    }
                }


                //List<string> listCatNames = new List<string>();
                //Dictionary<string, Element> dictUniqueElement = new Dictionary<string, Element>();

                string catName = doc.GetElement(selCollectionId.First()).Category.Name;
                
                foreach (ElementId elid in selCollectionId)
                {
                    var elem = doc.GetElement(elid);
                    
                    if (elem.Category.Name != catName)
                    {
                        System.Windows.MessageBox.Show("Выберите элементы одной категории");
                        return Result.Cancelled;
                    }                    

                    MyElement myelem = new MyElement(elem);
                    colSelElems.Add(myelem);
                    //listCatNames.Add(elem.Category.Name);

                    //try { dictUniqueElement.Add(elem.Category.Name, elem);} catch { }                    
                }

                Element elem1 = doc.GetElement(selCollectionId.First());
                int catId = elem1.Category.Id.IntegerValue;
                ParameterSet parameters = elem1.Parameters;
                List<Parameter> listParameters = new List<Parameter>();
                foreach (Parameter par in parameters)
                {
                    if (par.StorageType != StorageType.None)
                        listParameters.Add(par);
                }
                listParameters = listParameters.OrderBy(x => x.Definition.Name).ToList();

                foreach (Parameter par in listParameters)
                {
                    colParameters.Add(new MyParameter(par));
                }
                
                //foreach (KeyValuePair<string, Element> pair in dictUniqueElement)
                //{
                //    ViewModelCategories vmChild = new ViewModelCategories();
                    
                //    vmChild.Category = pair.Key;
                //    ObservableCollection<MyParameter> colParameters = new ObservableCollection<MyParameter>();
                //    List<MyParameter> listPararmeters = new List<MyParameter>();
                //    foreach (Parameter par in pair.Value.Parameters)
                //    {
                //        listPararmeters.Add(new MyParameter(par));
                //    }
                //    li stPararmeters = listPararmeters.OrderBy(x => x.Name).ToList();
                //    foreach (MyParameter mypar in listPararmeters)
                //    {
                //        colParameters.Add(mypar);
                //    }
                //    vmChild.ColParams = colParameters;

                //    colVMChilds.Add(vmChild);
                //}
                //listCatNames = listCatNames.Distinct().ToList();
                //foreach (string name in listCatNames)
                //{
                //    colCatNames.Add(name);
                //}

                ViewModelTH vmod = new ViewModelTH();

                vmod.ColSelectedElements = colSelElems;
                //vmod.ColCatNames = colCatNames;
                vmod.SelectedCategoryId = catId;
                vmod.ColParameters = colParameters;

                vmod.RevitModel = new ModelTH(uiapp);

                System.Diagnostics.Process proc = System.Diagnostics.Process.GetCurrentProcess();

                using (ViewTH view = new ViewTH())
                {
                    System.Windows.Interop.WindowInteropHelper helper = new System.Windows.Interop.WindowInteropHelper(view);
                    helper.Owner = proc.MainWindowHandle;

                    vmod.View = view;

                    view.DataContext = vmod;
                    //view.viewModel = vmod;

                    if (view.ShowDialog() != true)
                    {
                        return Result.Cancelled;
                    }
                }

                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return Result.Failed;
            }
        }
    }
}
