using System;
using System.ComponentModel.DataAnnotations;

namespace DeportesWeb.Models
{
    public class ReporteModel
    {
        [Required(ErrorMessage = "El campo Fecha Inicio es requerido")]
        [Display(Name = "FECHA INICIO")]
        public string fechaInicio { get; set; }
        [Required(ErrorMessage = "El campo Fecha Fin es requerido")]
        [Display(Name = "FECHA FIN")]
        public string fechaFin { get; set; }
    }
}