#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

#endregion

namespace Level_Creation
{
    [Transaction(TransactionMode.Manual)]
    public class Command1 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // this is a variable for the Revit application
            UIApplication uiapp = commandData.Application;

            // this is a variable for the current Revit model
            Document doc = uiapp.ActiveUIDocument.Document;

            // Your code goes here
            int num1 = 250;
            int elev = 0;
            int floor = 15;


            Transaction t = new Transaction(doc);
            t.Start("Level");
            for (int i = 1; i <= num1; i++)
            {
                Level name1 = Level.Create(doc, elev);
                name1.Name = "Level" + i.ToString();
                FilteredElementCollector collec = new FilteredElementCollector(doc);
                collec.OfClass(typeof(ViewFamilyType));


                if (i % 3 == 0)
                {
                    ViewFamilyType vfamtyp = null;
                    foreach (ViewFamilyType vf in collec)
                    {
                        if (vf.ViewFamily == ViewFamily.FloorPlan)
                        {
                            vfamtyp = vf;
                            break;
                        }
                    }
                ViewPlan newplan = ViewPlan.Create(doc, vfamtyp.Id, name1.Id);
                newplan.Name = "FIZZ_" + i.ToString(); ;
                }

                if (i % 5 == 0)
                {
                    ViewFamilyType vfamtyp = null;
                    foreach (ViewFamilyType vf in collec)
                    {
                        if (vf.ViewFamily == ViewFamily.CeilingPlan)
                        {
                            vfamtyp = vf;
                            break;
                        }
                    }
                    ViewPlan newplan = ViewPlan.Create(doc, vfamtyp.Id, name1.Id);
                    newplan.Name = "BUZZ_" + i.ToString(); 
                }
                if (i % 3 == 0 && i % 5 == 0)
                 {
                    FilteredElementCollector collec1 = new FilteredElementCollector(doc);
                    collec1.OfCategory(BuiltInCategory.OST_TitleBlocks);
                    ViewSheet viewSheet = ViewSheet.Create(doc, collec1.FirstElementId());
                    viewSheet.Name = "FIZZBUZZ_" + i.ToString();
                    viewSheet.SheetNumber = i.ToString();

                    //ViewFamilyType vfamtyp = null;
                    //foreach (ViewFamilyType vf in collec)
                    //{
                    //    if (vf.ViewFamily == ViewFamily.FloorPlan)
                    //    {
                    //        vfamtyp = vf;
                    //        break;
                    //    }
                    //}
                    //ViewPlan newplan = ViewPlan.Create(doc, vfamtyp.Id, name1.Id);
                    //newplan.Name = "FIZZBUZZ" + i.ToString(); ;
                    //XYZ poin = new XYZ(1, 1, 0);
                    //Viewport vport = Viewport.Create(doc, viewSheet.Id, newplan.Id,poin);
                }
                    elev += floor;
               
            }
            
            t.Commit();
            t.Dispose();


            return Result.Succeeded;
        }

        public static String GetMethod()
        {
            var method = MethodBase.GetCurrentMethod().DeclaringType?.FullName;
            return method;
        }
    }
}
