using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace DeportesWeb.Models
{
    public class SportUsers
    {
        public int id { get; set; }
        public string idUser { get; set; }
        [Required(ErrorMessage = "El campo Deporte es requerido")]
        [Display(Name = "DEPORTE")]
        public string nombre { get; set; }
    }
}