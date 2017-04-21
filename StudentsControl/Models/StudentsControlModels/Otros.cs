using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentsControl.Models.StudentsControlModels
{
    public class MapeoColumnas
    {
        public string ColDestino { get; set; }
        public string ColOrigen { get; set; }
        public string Tipo { get; set; }
        public string PrimaryKey { get; set; }
    }

    public class NombresColumnas
    {
        public string NombreExcel { get; set; }
        public string NombreColumna { get; set; }
    }
}