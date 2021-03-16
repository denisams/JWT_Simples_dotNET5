using System.ComponentModel.DataAnnotations;

namespace JWT.app.Autenticacao
{
    public class CadastroModelo
    {
        [Required(ErrorMessage = "Usuário é obrigatório")]
        public string NomeUsuario { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email é obrigatório")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Senha é obrigatória")]
        public string Senha { get; set; }
    }
}
