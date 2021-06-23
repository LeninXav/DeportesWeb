using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeportesWeb.Models
{
    public class SportsUsersViewModel
    {
        public int id { get; set; }
        public string idUser { get; set; }
        public string nombre { get; set; }
        public int frecuencias { get; set; }
    }
}