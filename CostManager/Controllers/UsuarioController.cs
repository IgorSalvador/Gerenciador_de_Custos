using CostManager.Models.Database;
using CostManager.Models.Utils;
using CostManager.Models.ViewModel;
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

        [HttpGet]
        public ActionResult Create()
        {
            return View(new UsuarioViewModel());
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Create(UsuarioViewModel model)
        {
            if (model.Perfil > 0)
                ViewBag.Perfil = model.Perfil;

            if (!ModelState.IsValid)
                return View(model);

            if (db.Usuario.Any(u => u.CPF.ToLower() == model.CPF.ToLower()))
            {
                ModelState.AddModelError("CPF", "CPF já cadastrado.");
                return View(model);
            }

            if (db.Usuario.Any(u => u.Email.ToLower() == model.Email.ToLower()))
            {
                ModelState.AddModelError("Email", "E-mail já cadastrado.");
                return View(model);
            }

            Usuario usuario = new Usuario
            {
                Nome = model.Nome,
                CPF = model.CPF,
                DataNascimento = model.DataNascimento,
                Email = model.Email,
                Senha = Utils.GetMD5Hash(Utils.GenerateRandomPassword()),
                Perfil = model.Perfil,
                Status = model.Status
            };
            db.Usuario.Add(usuario);
            db.SaveChanges();

            TempData["Message"] = "Usuário cadastrado com sucesso.";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public ActionResult Edit(int Id)
        {
            Usuario usuario = db.Usuario.Find(Id);

            if (usuario == null)
            {
                TempData["Message"] = "Usuário não encontrado.";
                return RedirectToAction(nameof(Index));
            }

            return View(new UsuarioViewModel(usuario));      
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(int Id, UsuarioViewModel model)
        {
            if (model.Perfil > 0)
                ViewBag.Perfil = model.Perfil;

            if (!ModelState.IsValid)
                return View(model);

            Usuario usuario = db.Usuario.Find(Id);

            if (usuario == null)
            {
                TempData["Message"] = "Usuário não encontrado.";
                return RedirectToAction(nameof(Index));
            }

            if (db.Usuario.Any(u => u.CPF.ToLower() == model.CPF.ToLower() && u.Id != Id))
            {
                ModelState.AddModelError("CPF", "CPF já cadastrado.");
                return View(model);
            }

            if (db.Usuario.Any(u => u.Email.ToLower() == model.Email.ToLower() && u.Id != Id))
            {
                ModelState.AddModelError("Email", "E-mail já cadastrado.");
                return View(model);
            }

            usuario.Nome = model.Nome;
            usuario.CPF = model.CPF;
            usuario.DataNascimento = model.DataNascimento;
            usuario.Email = model.Email;
            usuario.Perfil = model.Perfil;
            usuario.Status = model.Status;

            db.SaveChanges();

            TempData["Message"] = "Usuário alterado com sucesso.";

            return RedirectToAction(nameof(Index));
        }
    }
}