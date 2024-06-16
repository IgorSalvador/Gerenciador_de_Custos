using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CostManager.Models
{
    public enum Perfis
    {
        Administrador = 1,
        Usuario = 2
    }

    public static class PerfisExtensions
    {
        public static string GetDescription(int perfil)
        {
            switch (perfil)
            {
                case (int)Perfis.Administrador:
                    return "Administrador";
                case (int)Perfis.Usuario:
                    return "Usuário";
                default:
                    return string.Empty;
            }
        }
    }
}