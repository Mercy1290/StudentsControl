using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace StudentsControl.Models
{
    [Table("CentrosEducativos")]
    public class CentroEducativo
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required, MaxLength(20)]
        [Display(Name = "Código")]
        public string CodCentro { get; set; }
        [Required, MaxLength(120)]
        [Display(Name = "Centro Educativo")]
        public string NomCentro { get; set; }
        [MaxLength(255)]
        [Display(Name = "Dirección")]
        public string Direccion { get; set; }
        [Required, MaxLength(100)]
        public string Ciudad { get; set; }
        [MaxLength(100)]
        public string Departamento { get; set; }
        [Required, MaxLength(20)]
        [Display(Name = "Pais")]
        public string Pais { get; set; }
    }
}
