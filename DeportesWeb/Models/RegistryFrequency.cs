using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DeportesWeb.Models
{
    public class RegistryFrequency
    {
        public int id { get; set; }
        [Required(ErrorMessage = "El campo Deporte es requerido")]
        [Display(Name = "DEPORTE")]
        public int idSportUser { get; set; }
        [Required(ErrorMessage = "El campo Frecuencia es requerido")]
        [Display(Name = "FRECUENCIA")]
        public string frecuencia { get; set; }
        [Required(ErrorMessage = "El campo Horas es requerido")]
        [Display(Name = "HORAS")]
        public int tiempo { get; set; }
    }
}