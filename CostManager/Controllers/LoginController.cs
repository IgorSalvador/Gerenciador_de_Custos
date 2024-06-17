using CostManager.Models;
using CostManager.Models.Database;
using CostManager.Models.Utils;
using CostManager.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace CostManager.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        private readonly dbCostManagerEntities db = new dbCostManagerEntities();

        [HttpGet]
        public ActionResult Index()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");
            else
                return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Index(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            string senha = Utils.GetMD5Hash(model.Senha);

            Usuario usuario = db.Usuario.Where(x => x.Email.ToUpper() == model.Email.ToUpper() &&
                        x.Senha == senha).FirstOrDefault();

            if (usuario == null)
            {
                ModelState.AddModelError("Email", "Usuário ou senha inválidos.");
                return View(model);
            }

            var claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.Name, usuario.Nome));
            claims.Add(new Claim(ClaimTypes.Email, usuario.Email));
            claims.Add(new Claim(ClaimTypes.Role, PerfisExtensions.GetDescription(usuario.Perfil)));

            MvcApplication objGlobal = new MvcApplication();
            objGlobal.StartSession(claims);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet, Authorize]
        public ActionResult LogOut()
        {
            //Logout removendo Claims
            var ctx = Request.GetOwinContext();
            var authenticationManager = ctx.Authentication;
            authenticationManager.SignOut();

            // Limpa o cookie de autenticação
            FormsAuthentication.SignOut();

            // Remove todas as claims do usuário
            var identity = (ClaimsIdentity)User.Identity;
            foreach (var claim in identity.Claims.ToList())
                identity.RemoveClaim(claim);

            Session.Abandon();

            return RedirectToAction("Index", "Login");
        }
    }
}