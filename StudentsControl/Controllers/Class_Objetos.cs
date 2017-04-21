using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;


namespace StudentsControl.Controllers
{
    public class Class_Objetos
    {
        public static int GetRowCount(SpreadsheetDocument spreadsheetdocument, string SheetName)
        {
            try
            {
                string relationshipId = "";
                try
                {
                    relationshipId = spreadsheetdocument.WorkbookPart.Workbook.Descendants<Sheet>().First(s => SheetName.Equals(s.Name)).Id;
                }
                catch (Exception)
                {
                    relationshipId = spreadsheetdocument.WorkbookPart.Workbook.Descendants<Sheet>().First().Id.Value;
                }

                WorksheetPart worksheetPart = (WorksheetPart)spreadsheetdocument.WorkbookPart.GetPartById(relationshipId);
                Worksheet workSheet = worksheetPart.Worksheet;
                SheetData sheetData = workSheet.GetFirstChild<SheetData>();
                IEnumerable<Row> rows = sheetData.Descendants<Row>();
                return rows.Count();
            }
            catch (Exception)
            {
                return 0;
            }
        }


    }
}