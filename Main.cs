using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAPI3._2
{
    [Transaction(TransactionMode.Manual)]
    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            IList<Reference> selectedElementRefList = uidoc.Selection.PickObjects(ObjectType.Element, new PipeFilter(), "Выберете элементы труб");
            var pipeList = new List<Pipe>();
            var length = new List<double>();
            double sum = 0;
            string Length = string.Empty;

            foreach (var selectedElement in selectedElementRefList)
            {
                Pipe oPipe = doc.GetElement(selectedElement) as Pipe;
                pipeList.Add(oPipe);
            }
            foreach (var pipe in pipeList)
            {
                Parameter pipeLength = pipe.get_Parameter(BuiltInParameter.CURVE_ELEM_LENGTH);
                double V = UnitUtils.ConvertFromInternalUnits(pipeLength.AsDouble(), UnitTypeId.Millimeters);
                length.Add(V);
            }
            foreach (var i in length)
            {
                sum += i;
            }
            string SumLength = sum.ToString();
            Length += $"Длина: {SumLength}мм{Environment.NewLine}Общее количество элементов труб: {pipeList.Count}";

            TaskDialog.Show("Selection", Length);

            return Result.Succeeded;
        }
    }
}
