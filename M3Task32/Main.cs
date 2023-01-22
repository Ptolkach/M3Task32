using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M3Task32
{
    [Transaction(TransactionMode.Manual)]
    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            IList<Reference> selectedElementRefList = uidoc.Selection.PickObjects(ObjectType.Element, new PipeFilter(), "Выберите трубы");
            var pipeLengthParamList = new List<Parameter>();

            foreach (var selectedElement in selectedElementRefList)
            {
                Element element = doc.GetElement(selectedElement);
                Pipe oPipe = (Pipe)element;
                pipeLengthParamList.Add(oPipe.get_Parameter(BuiltInParameter.CURVE_ELEM_LENGTH));
            }

            var pipeLengthList_SI = new List<Double>();

            foreach (var len in pipeLengthParamList)
            {
                pipeLengthList_SI.Add(UnitUtils.ConvertFromInternalUnits(len.AsDouble(), UnitTypeId.Meters));
            }

            TaskDialog.Show("Длина выбранных труб", $"{pipeLengthList_SI.Sum().ToString()}, м");


            return Result.Succeeded;
        }
    }
}
