using CostManager.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CostManager.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class UsuarioController : Controller
    {
        private readonly dbCostManagerEntities db = new dbCostManagerEntities();
        private const int pageSize = 10;

        [HttpGet]
        public ActionResult Index(string nome, string email, string status, string perfil, int pagina = 1)
        {
            List<Usuario> usuarios = db.Usuario.ToList();

            if (!string.IsNullOrEmpty(nome))
                usuarios = usuarios.Where(u => u.Nome.Contains(nome)).ToList();

            if (!string.IsNullOrEmpty(email))
                usuarios = usuarios.Where(u => u.Email.Contains(email)).ToList();

            if (!string.IsNullOrEmpty(status))
            {
                bool Status = status == "1" ? true : false;
                usuarios = usuarios.Where(u => u.Status == Status).ToList();
            }

            if (!string.IsNullOrEmpty(perfil))
            {
                int Perfil = Convert.ToInt32(perfil);
                usuarios = usuarios.Where(u => u.Perfil == Perfil).ToList();
            }

            ViewBag.Nome = nome ?? string.Empty;
            ViewBag.Email = email ?? string.Empty;
            ViewBag.Status = status ?? string.Empty;
            ViewBag.Perfil = perfil ?? string.Empty;
            ViewBag.Pagina = pagina;
            ViewBag.TotalPaginas = Math.Ceiling((double)usuarios.Count / 10);

            return View(usuarios.Skip((pagina - 1) * pageSize).Take(pageSize));
        }
    }
}