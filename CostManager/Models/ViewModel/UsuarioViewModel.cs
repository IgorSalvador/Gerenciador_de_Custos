using CostManager.Models.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CostManager.Models.ViewModel
{
    public class UsuarioViewModel
    {
        [DisplayName("Nome")]
        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(250, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 250 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [DisplayName("E-mail")]
        [Required(ErrorMessage = "O e-mail é obrigatório")]
        [EmailAddress(ErrorMessage = "E-mail inválido")]
        [StringLength(250, MinimumLength = 3, ErrorMessage = "O e-mail deve ter entre 3 e 250 caracteres")]
        public string Email { get; set; } = string.Empty;

        [DisplayName("CPF")]
        [Required(ErrorMessage = "O CPF é obrigatório")]
        [StringLength(20, MinimumLength = 11, ErrorMessage = "O CPF deve ter 11 caracteres")]
        public string CPF { get; set; } = string.Empty;

        [DisplayName("Data de Nascimento")]
        [Required(ErrorMessage = "A data de nascimento é obrigatória")]
        public DateTime DataNascimento { get; set; } = DateTime.Now;

        [DisplayName("Perfil")]
        [Required(ErrorMessage = "O perfil é obrigatório")]
        [Range(1, 2, ErrorMessage = "Perfil inválido")]
        public int Perfil { get; set; } = 2; // 1 - Administrador, 2 - Usuário

        [DisplayName("Status")]
        public bool Status { get; set; } = true;

        public UsuarioViewModel()
        {
            
        }

        public UsuarioViewModel(Usuario usuario)
        {
            Nome = usuario.Nome;
            Email = usuario.Email;
            CPF = usuario.CPF;
            DataNascimento = usuario.DataNascimento;
            Perfil = usuario.Perfil;
            Status = usuario.Status;
        }
    }
}