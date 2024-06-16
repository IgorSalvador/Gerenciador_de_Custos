using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CostManager.Models.ViewModel
{
    public class LoginViewModel
    {
        [DisplayName("E-mail")]
        [EmailAddress(ErrorMessage = "E-mail inválido!")]
        [Required(ErrorMessage = "Preencha o campo E-mail!")]
        public string Email { get; set; } = string.Empty;

        [DisplayName("Senha")]
        [Required(ErrorMessage = "Preencha o campo Senha!")]
        public string Senha { get; set; } = string.Empty;
    }
}