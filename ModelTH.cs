using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using RVTAppServ = Autodesk.Revit.ApplicationServices.Application;

namespace TemporaryHiding
{
    [Transaction(TransactionMode.Manual)]
    class ModelTH
    {
        private UIApplication UIAPP = null;
        private RVTAppServ APP = null;
        private UIDocument UIDOC = null;
        private Document DOC = null;

        public ModelTH(UIApplication uiapp)
        {
            UIAPP = uiapp;
            APP = UIAPP.Application;
            UIDOC = uiapp.ActiveUIDocument;
            DOC = UIDOC.Document;
        }

        public bool HideOrIsolateElems(int catId, MyParameter myParameter, ObservableCollection<MyElement> colElements, bool HideOrIsolate)
        {
            List<ElementId> categoryIds = CategoryIds(catId);

            List<ElementId> elementIds = new List<ElementId>();
            try
            {
                using (Transaction tx = new Transaction(DOC))
                {
                    tx.Start("Start");
                    foreach (ElementId categoryId in categoryIds)
                    {
                        List<string> parameterValues = new List<string>();
                        foreach (MyElement myElement in colElements)
                        {
                            Element elem = myElement.Element;
                            Parameter parameter = myParameter.Parameter;
                            parameterValues.Add(ParamValue(elem.LookupParameter(parameter.Definition.Name)));
                        }

                        foreach (MyElement myElement in colElements)
                        {
                            Element elem = myElement.Element;
                            Parameter parameter = myParameter.Parameter;
                            parameterValues.Add(ParamValue(elem.LookupParameter(parameter.Definition.Name)));


                            if (HideOrIsolate)
                            {
                                var collector = new FilteredElementCollector(DOC)
                                    .WhereElementIsNotElementType()
                                    .OfCategoryId(categoryId)
                                    .Where(x => x.ParametersMap.Contains(parameter.Definition.Name))
                                    .Where(x => parameterValues.Contains(ParamValue(x.LookupParameter(parameter.Definition.Name)))==true)
                                    .Select(x => x.Id)
                                    .ToList();
                                elementIds.AddRange(collector);
                            }

                            else
                            {
                                var collector = new FilteredElementCollector(DOC)
                                    .WhereElementIsNotElementType()
                                    .OfCategoryId(categoryId)
                                    .Where(x => x.ParametersMap.Contains(parameter.Definition.Name))
                                    .Where(x => parameterValues.Contains(ParamValue(x.LookupParameter(parameter.Definition.Name))) == false)
                                    .Select(x => x.Id)
                                    .ToList();
                                elementIds.AddRange(collector);
                            }

                        }
                    }
                    DOC.ActiveView.HideElementsTemporary(elementIds);
                    tx.Commit();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
            return true;
        }        

        private string ParamValue(Parameter parameter)
        {
            string res = "";
            switch (parameter.StorageType)
            {
                case StorageType.Double:
                    res = parameter.AsValueString();
                    break;
                case StorageType.Integer:
                    if (parameter.Definition.ParameterType == ParameterType.YesNo)
                        res = parameter.AsValueString();
                    else
                        res = parameter.AsInteger().ToString();
                    break;
                case StorageType.ElementId:
                    res = parameter.AsValueString();
                    break;
                default:
                    res = parameter.AsString();
                    break;
            }
            return res;
        }
        private List<ElementId> CategoryIds(int catId)
        {
            List<ElementId> categoryIds = new List<ElementId>();

            if (catId == -2008000 | catId == -2008016 | catId == -2008013 | catId == -2008010 | catId == -2008020)
            {
                categoryIds.Add(new ElementId(-2008000));
                categoryIds.Add(new ElementId(-2008016));
                categoryIds.Add(new ElementId(-2008013));
                categoryIds.Add(new ElementId(-2008010));
                categoryIds.Add(new ElementId(-2008020));
            }
            else if (catId == -2008044 | catId == -2008050 | catId == -2008055 | catId == -2001160 | catId == -2008049)
            {
                categoryIds.Add(new ElementId(-2008044));
                categoryIds.Add(new ElementId(-2008050));
                categoryIds.Add(new ElementId(-2008055));
                categoryIds.Add(new ElementId(-2001160));
                categoryIds.Add(new ElementId(-2008049));
            }
            else
                categoryIds.Add(new ElementId(catId));

            return categoryIds;
        }
        //public ObservableCollection<MyElement> Select()
        //{
        //    IList<Reference> picked = null;

        //    ObservableCollection<MyElement> colSelElems = new ObservableCollection<MyElement>();

        //    Selection sel = UIDOC.Selection;
        //    try
        //    {
        //        picked = sel.PickObjects(ObjectType.Element);
        //    }
        //    catch { }

        //    foreach (Reference refer in picked)
        //    {
        //        Element elem = DOC.GetElement(refer);
        //        colSelElems.Add(new MyElement(elem));
        //    }

        //    return colSelElems;
        //}

    }
}
