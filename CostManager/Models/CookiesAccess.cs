using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace CostManager.Models
{
    public class CookiesAccess
    {
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Perfil { get; set; } = string.Empty;
        public bool IsLoggedIn { get; set; } = false;
        public bool IsLoginExpired { get; set; } = true;

        public CookiesAccess()
        {
            try
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    // Obtém a identidade atual do usuário autenticado
                    ClaimsIdentity identity = (ClaimsIdentity)HttpContext.Current.User.Identity;

                    // Obtém as claims do usuário
                    var claims = identity.Claims;

                    this.Nome = claims.Where(x => x.Type == ClaimTypes.Name).First().Value;
                    this.Email = claims.Where(x => x.Type == ClaimTypes.Email).First().Value;
                    this.Perfil = claims.Where(x => x.Type == ClaimTypes.Role).First().Value;
                    this.IsLoggedIn = true;
                    this.IsLoginExpired = false;
                }
                else
                {
                    throw new Exception("Usuário não autenticado.");
                }
            }
            catch
            {
                this.Nome = string.Empty;
                this.Email = string.Empty;
                this.Perfil = string.Empty;
                this.IsLoggedIn = false;
                this.IsLoginExpired = true;
            }
        }
    }

    public static class CurrentCookies
    {
        public static string Nome { get; set; }
        public static string Email { get; set; }
        public static string Perfil { get; set; }
        public static bool IsLoggedIn { get; set; }

        public static void getCookies()
        {
            CookiesAccess cookies = new CookiesAccess();

            Nome = cookies.Nome;
            Email = cookies.Email;
            Perfil = cookies.Perfil;
            IsLoggedIn = cookies.IsLoggedIn;
        }
    }
}