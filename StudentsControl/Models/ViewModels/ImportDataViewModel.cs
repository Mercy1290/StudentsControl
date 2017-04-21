using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentsControl.Models.ViewModels
{
    public class ImportDataViewModel
    {
        public string Nombre { get; set; }
        public List<string> Hojas { get; set; }
    }
}