using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeportesWeb.Models
{
    public class ReporteTabla
    {
        public int id { get; set; }
        public string deporte { get; set; }
        public int tiempo { get; set; }
        public string frecuencia { get; set; }
    }
}