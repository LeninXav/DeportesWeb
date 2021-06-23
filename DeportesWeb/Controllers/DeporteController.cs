using DeportesWeb.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using static DeportesWeb.Models.Contexto;

namespace DeportesWeb.Controllers
{
    [Authorize]
    public class DeporteController : Controller
    {
        private SportUsersDBContext db;
        private SportsDBContext db2;
        private FrequencyDBContext db3;
        private SportRegistryDBContext db4;
        private RegistryFrequencyDBContext db5;

        public DeporteController()
        {
            db = new SportUsersDBContext();
            db2 = new SportsDBContext();
            db3 = new FrequencyDBContext();
            db4 = new SportRegistryDBContext();
            db5 = new RegistryFrequencyDBContext(); 
        }

        // GET: Deporte
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var deportes = db.SportUser.Where(s => s.idUser == userId).ToList();
            List<SportsUsersViewModel> lista = new List<SportsUsersViewModel>();
            foreach (var item in deportes)
            {
                SportsUsersViewModel model = new SportsUsersViewModel();
                model.id = item.id;
                model.nombre = item.nombre;
                var frecuencias = db5.RegistryFrequency.Where(rf => rf.idSportUser == item.id).ToList();
                model.frecuencias = frecuencias.Count();
                lista.Add(model);
            }
            TempData["idUser"] = User.Identity.GetUserId();
            return View(lista);
        }

        public ActionResult Create()
        {
            var userId = User.Identity.GetUserId();
            var nombres = db.SportUser.Where(s => s.idUser == userId).Select(s => s.nombre).ToList();
            var deportes = db2.Sports.ToList();
            IList<SelectListItem> listaVacia = new List<SelectListItem>();
            listaVacia.Add(new SelectListItem { Text = "seleccione", Value = null });
            foreach (var item in deportes)
            {
                if (!nombres.Contains(item.nombre))
                    listaVacia.Add(new SelectListItem { Text = item.nombre, Value = item.nombre });
            }
            ViewBag.Deportes = new SelectList(listaVacia, "Value", "Text");
            TempData["idUser"] = User.Identity.GetUserId();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "nombre")] SportUsers sportUser)
        {
            var userId = User.Identity.GetUserId();
            sportUser.idUser = userId;
            if (ModelState.IsValid)
            {
                db.SportUser.Add(sportUser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            TempData["idUser"] = User.Identity.GetUserId();
            return View(sportUser);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SportUsers sportUser = db.SportUser.Find(id);
            if (sportUser == null)
            {
                return HttpNotFound();
            }
            TempData["idUser"] = User.Identity.GetUserId();
            return View(sportUser);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SportUsers sportUser = db.SportUser.Find(id);
            var registros = db4.SportRegistry.Where(c => c.idSportUser == id).ToList();
            var registrosF = db5.RegistryFrequency.Where(rf => rf.idSportUser == id).ToList();
            TempData["idUser"] = User.Identity.GetUserId();
            if (registros.Count == 0 && registrosF.Count == 0)
            {
                db.SportUser.Remove(sportUser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            TempData["Error"] = "No se puede eliminar tiene datos";
            return RedirectToAction("DeleteError");
        }

        public ActionResult DeleteError()
        {
            TempData["idUser"] = User.Identity.GetUserId();
            return View();
        }

        public ActionResult IndexRegistry()
        {
            var userId = User.Identity.GetUserId();
            var deportes = db.SportUser.Where(s => s.idUser == userId).ToList();
            List<SportRegistryViewModel> lista = new List<SportRegistryViewModel>();
            foreach (var item in deportes)
            {
                SportRegistryViewModel model = new SportRegistryViewModel();
                model.id = item.id;
                model.nombre = item.nombre;
                var frecuencias = db5.RegistryFrequency.Where(rf => rf.idSportUser == item.id).ToList();
                model.frecuencias = frecuencias.Count();
                var registros = db4.SportRegistry.Where(rg => rg.idSportUser == item.id).ToList();
                model.registros = registros.Count();
                lista.Add(model);
            }
            TempData["idUser"] = User.Identity.GetUserId();
            return View(lista);
        }

        public ActionResult Registry(int id)
        {
            var userId = User.Identity.GetUserId();
            IList<SelectListItem> listaVacia2 = new List<SelectListItem>();
            listaVacia2.Add(new SelectListItem { Text = "seleccione", Value = null });
            var frecuencias = db5.RegistryFrequency.Where(rf => rf.idSportUser == id).ToList();
            foreach (var item in frecuencias)
            {
                listaVacia2.Add(new SelectListItem { Text = item.frecuencia, Value = item.frecuencia });
            }
            TempData["id"] = id;
            TempData["idUser"] = User.Identity.GetUserId();
            ViewBag.Frecuencias = new SelectList(listaVacia2, "Value", "Text");
            return View();
        }

        public ActionResult RegistryError()
        {
            TempData["idUser"] = User.Identity.GetUserId();
            return View();
        }

        public ActionResult RegistryFrequencyError()
        {
            TempData["idUser"] = User.Identity.GetUserId();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registry([Bind(Include = "idSportUser,frecuencia,tiempo,fecha")] SportRegistry sportRegistry)
        {
            if (ModelState.IsValid)
            {
                var registro = db4.SportRegistry.Where(sr => sr.idSportUser == sportRegistry.idSportUser && sr.fecha == sportRegistry.fecha).FirstOrDefault();
                if (registro == null)
                {
                    db4.SportRegistry.Add(sportRegistry);
                    db4.SaveChanges();
                    TempData["idUser"] = User.Identity.GetUserId();
                    return RedirectToAction("IndexRegistry");
                }
                TempData["idUser"] = User.Identity.GetUserId();
                TempData["Error"] = "Ya se registro una actividad en la fecha: " + registro.fecha.ToShortDateString();
                return RedirectToAction("RegistryError");
            }
            TempData["idUser"] = User.Identity.GetUserId();
            return View(sportRegistry);
        }

        public ActionResult IndexRegistryFrequecy(int? id)
        {
            var userId = User.Identity.GetUserId();
            var registrosFrecuencia = db5.RegistryFrequency.Where(rf => rf.idSportUser == id).ToList();
            List<RegistryFrequencyViewModel> lista = new List<RegistryFrequencyViewModel>();
            foreach (var item in registrosFrecuencia)
            {
                RegistryFrequencyViewModel model = new RegistryFrequencyViewModel();
                model.id = item.id;
                model.frecuencia = item.frecuencia;
                model.tiempo = item.tiempo;
                lista.Add(model);
            }
            TempData["idUser"] = User.Identity.GetUserId();
            TempData["deporte"] = db.SportUser.Where(su => su.id == id).Select(su => su.nombre).FirstOrDefault();
            TempData["id"] = id;
            return View(lista);
        }

        public ActionResult CreateRegistryFrequecy(int? id)
        {
            var userId = User.Identity.GetUserId();
            var frecuencias = db3.Frequency.ToList();
            var frecuenciasAsignadas = db5.RegistryFrequency.Where(rf => rf.idSportUser == id).Select(rf => rf.frecuencia).ToList();
            IList<SelectListItem> listaVacia = new List<SelectListItem>();
            listaVacia.Add(new SelectListItem { Text = "seleccione", Value = null });
            foreach (var item in frecuencias)
            {
                if (!frecuenciasAsignadas.Contains(item.nombre))
                {
                    listaVacia.Add(new SelectListItem { Text = item.nombre, Value = item.nombre });
                }
            }
            ViewBag.Frecuencias = new SelectList(listaVacia, "Value", "Text");
            TempData["idUser"] = User.Identity.GetUserId();
            TempData["id"] = id;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateRegistryFrequecy([Bind(Include = "idSportUser,frecuencia,tiempo")] RegistryFrequency sportUserFrequency)
        {
            var userId = User.Identity.GetUserId();
            if (ModelState.IsValid)
            {
                var registro = db5.RegistryFrequency.Where(rf => rf.idSportUser == sportUserFrequency.idSportUser).FirstOrDefault();
                if (registro == null)
                {
                    db5.RegistryFrequency.Add(sportUserFrequency);
                    db5.SaveChanges();
                    TempData["idUser"] = User.Identity.GetUserId();
                    return RedirectToAction("IndexRegistryFrequecy", new { id = sportUserFrequency.idSportUser });
                }
                else
                {
                    TempData["Error2"] = "Agregar";
                    TempData["idUser"] = User.Identity.GetUserId();
                    TempData["Error1"] = "Error al crear una frecuencia en este Deporte";
                    TempData["Error"] = "Ya existe registrada una frecuencia.";
                    TempData["id"] = sportUserFrequency.idSportUser;
                    return RedirectToAction("RegistryFrequencyError");
                }
                
            }
            TempData["idUser"] = User.Identity.GetUserId();
            TempData["id"] = sportUserFrequency.idSportUser;
            return View(sportUserFrequency);
        }

        public ActionResult DeleteRegistryFrequecy(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RegistryFrequency sportUser = db5.RegistryFrequency.Find(id);
            if (sportUser == null)
            {
                return HttpNotFound();
            }
            TempData["idUser"] = User.Identity.GetUserId();
            return View(sportUser);
        }

        [HttpPost, ActionName("DeleteRegistryFrequecy")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteFrequencyConfirmed(int id)
        {
            RegistryFrequency sportUser = db5.RegistryFrequency.Find(id);
            var registros = db4.SportRegistry.Where(c => c.idSportUser == sportUser.idSportUser).ToList();
            TempData["idUser"] = User.Identity.GetUserId();
            if (registros.Count == 0)
            {
                db5.RegistryFrequency.Remove(sportUser);
                db5.SaveChanges();
                return RedirectToAction("Index");
            }
            TempData["Error2"] = "Eliminar";
            TempData["Error1"] = "Error al eliminar una frecuencia en este Deporte";
            TempData["id"] = sportUser.idSportUser;
            TempData["Error"] = "No se puede eliminar tiene datos";
            return RedirectToAction("RegistryFrequencyError");
        }

        public ActionResult Report()
        {
            TempData["idUser"] = User.Identity.GetUserId();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Report([Bind(Include = "fechaInicio,fechaFin")] ReporteModel reporte)
        {
            if (ModelState.IsValid)
            {
                var fechaInicio = Convert.ToDateTime(reporte.fechaInicio);
                var fechaFin = Convert.ToDateTime(reporte.fechaFin).AddDays(1);
                List<ReporteTabla> tabla = new List<ReporteTabla>();
                var idUser = User.Identity.GetUserId();
                var deportes = db.SportUser.Where(u => u.idUser == idUser).ToList();
                foreach (var item in deportes)
                {
                    ReporteTabla aux = new ReporteTabla();
                    aux.id = item.id;
                    aux.deporte = item.nombre;
                    var registros = db4.SportRegistry.Where(r => r.fecha >= fechaInicio && r.fecha <= fechaFin && r.idSportUser == item.id).ToList();
                    int tiempo = 0;
                    var frecuencia = "";
                    foreach (var item2 in registros)
                    {
                        tiempo += item2.tiempo;
                        frecuencia = item2.frecuencia;
                    }
                    aux.frecuencia = frecuencia;
                    aux.tiempo = tiempo;
                    tabla.Add(aux);

                }
                TempData["idUser"] = User.Identity.GetUserId();
                TempData["list"] = tabla.ToList();
                return RedirectToAction("DataReport", new { lista = tabla.ToList()});
            }
            TempData["idUser"] = User.Identity.GetUserId();
            return View(reporte);
        }

        public ViewResult DataReport(List<ReporteTabla> lista)
        {
            TempData["idUser"] = User.Identity.GetUserId();
            lista = TempData["list"] as List<ReporteTabla>;
            return View(lista);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
                db2.Dispose();
                db3.Dispose();
                db4.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}