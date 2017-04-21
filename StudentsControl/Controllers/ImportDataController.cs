using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StudentsControl.Models;
using System.Data.Entity.Core.Objects.DataClasses;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using System.Data;
using StudentsControl.Models.StudentsControlModels;
using EasyXLS;

namespace StudentsControl.Controllers
{
    public class ImportDataController : Controller
    {
        StudentsControlContext db = new StudentsControlContext();
        string directorio = "~/Content/Files/";

        // GET: ImportData
        public ActionResult Index()
        {
            ViewBag.Maestro = "CentrosEducativos";
            return View();
        }

        [HttpPost]
        public ActionResult Upload()
        {
            var myFile = Request.Files["myFile"];
            var targetLocation = Server.MapPath(directorio);

            try
            {
                var path = Path.Combine(targetLocation, Path.GetFileName(myFile.FileName));
                var ExtensionArchivo = Path.GetExtension(myFile.FileName);
                myFile.SaveAs(path);
                TempData["ExtensionArchivo"] = ExtensionArchivo;
            }
            catch
            {
                Response.StatusCode = 400;
            }

            return new EmptyResult();
        }
        public ActionResult ObtenerHojas(string nombreArchivo)
        {
            var targetLocation = Server.MapPath(directorio);
            var path = Path.Combine(targetLocation, Path.GetFileName(nombreArchivo));

            FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            SpreadsheetDocument spreadSheetDocument = SpreadsheetDocument.Open(path, false);
            TempData["documento"] = spreadSheetDocument;
            TempData["path"] = path;
            WorkbookPart workbookPart = spreadSheetDocument.WorkbookPart;
            IEnumerable<Sheet> sheets = spreadSheetDocument.WorkbookPart.Workbook.GetFirstChild<Sheets>().Elements<Sheet>();

            List<string> listaHojas = new List<string>();
            foreach (Sheet sheet in sheets)
            {
                listaHojas.Add(sheet.Name);
            }

            return Json(listaHojas, JsonRequestBehavior.AllowGet);

        }
        public ActionResult ObtenerRango(string nombreHoja)
        {
            int celdaIni = 0, celdaFin = 0;
            SpreadsheetDocument spreadSheetDocument = (SpreadsheetDocument)TempData["documento"];
            //Obtener Celda Inicio y Fin
            if (nombreHoja != "")
            {
                celdaIni = 1;
                celdaFin = Class_Objetos.GetRowCount(spreadSheetDocument, nombreHoja) - 1;
            }
            var resultJson = new { CeldaIni = celdaIni, CeldaFin = celdaFin };
            return Json(resultJson, JsonRequestBehavior.AllowGet);
        }
        public ActionResult MapeoCampos(string nombreHoja, int filaIni, int filaFin)
        {
            #region Obteniendo Campos de BD
            string instrSQL = "";
            string Tabla = "CentrosEducativos";

            //Ya el Usuario selecciono la tabla, entonces se pueden obtener los Campos en la BD que la conforman
            instrSQL = "SELECT c.COLUMN_NAME AS ColDestino,'' AS ColOrigen,c.DATA_TYPE AS Tipo\n" +
                       $",CASE WHEN k.CONSTRAINT_NAME IS NOT NULL THEN 'PK' END AS PrimaryKey\n" +
                       $"FROM INFORMATION_SCHEMA.COLUMNS c\n" +
                       $"LEFT JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE k ON c.COLUMN_NAME = k.COLUMN_NAME AND c.TABLE_NAME = k.TABLE_NAME AND c.TABLE_CATALOG = k.TABLE_CATALOG\n" +
                       $"WHERE c.TABLE_NAME = '{Tabla}' \n" +
                       $"AND c.TABLE_CATALOG='{db.Database.Connection.Database}'\n" +
                       $"AND ISNULL(k.CONSTRAINT_NAME,'N') NOT LIKE 'FK%';";
            IEnumerable<MapeoColumnas> data = db.Database.SqlQuery<MapeoColumnas>(instrSQL);
            var properties = data.ToArray();
            
            var ColBDArray = JsonConvert.SerializeObject(properties);
            #endregion
            #region Abriendo Hoja con EasyXLS
            //var stream = (FileStream)TempData["stream"];
            var path = (string)TempData["path"];
            var ExtensionArchivo = (string)TempData["ExtensionArchivo"];

            DataSet result = new DataSet();
            DataTable DTHoja = new DataTable();
            ExcelDocument workbook = new ExcelDocument();
            try
            {
                if (ExtensionArchivo == ".xls")
                    result = workbook.easy_ReadXLSSheet_AsDataSet(path, nombreHoja);
                if (ExtensionArchivo == ".xlsx")
                    result = workbook.easy_ReadXLSXSheet_AsDataSet(path, nombreHoja);
            }
            catch (Exception exs)
            {
                string error = workbook.easy_getError();
            }

            DTHoja = result.Tables[0];
            for (int c = 0; c < DTHoja.Columns.Count; c++)
            {
                string NombreCol = DTHoja.Rows[0][c].ToString();
                try
                {
                    DTHoja.Columns[c].ColumnName = NombreCol;
                }
                catch (Exception exs)
                { break; }
            }
            DTHoja.Rows.RemoveAt(0);
            #endregion

            string str = "";
            if (filaIni != 1) filaIni = filaIni - 2;
            if (filaIni < 0) filaIni = 0;
            List<NombresColumnas> ArrayColumnas = new List<NombresColumnas>();
            NombresColumnas nc = new NombresColumnas();
            nc.NombreColumna = "";
            nc.NombreExcel = "";
            ArrayColumnas.Add(nc);
            for (int c = 0; c < DTHoja.Columns.Count; c++)
            {
                if (filaIni == 1)
                {
                    if (DTHoja.Columns[c] != null)
                        str = DTHoja.Columns[c].ColumnName;
                }
                else
                {
                    if (DTHoja.Rows[filaIni][c] != null)
                        str = DTHoja.Rows[filaIni][c].ToString();
                }
                if (str != "" && str != null)
                {
                    str = str.Trim();
                    str = str.Replace("\"", "");
                    str = str.Replace("'", "");

                    nc = new NombresColumnas();
                    nc.NombreColumna = DTHoja.Columns[c].ColumnName;
                    nc.NombreExcel = str;
                    ArrayColumnas.Add(nc);
                }
            }//Fin Ciclo columnas en Rango Excel

            //var array = DT_cmb.AsEnumerable().ToArray();
            var lookUpjsonArray = JsonConvert.SerializeObject(ArrayColumnas);

            var resultJson = new { dgDataSource = ColBDArray, dgLookUp = lookUpjsonArray };
            return Json(resultJson, JsonRequestBehavior.AllowGet);

        }
    }
}