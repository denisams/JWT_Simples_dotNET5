using System.ComponentModel.DataAnnotations;

namespace JWT.app.Autenticacao
{
    public class CadastroModelo
    {
        [Required(ErrorMessage = "Usuário é obrigatório")]
        public string NomeUsuario { get; set; } = string.Empty;

        [EmailAddress]
        [Required(ErrorMessage = "Email é obrigatório")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Senha é obrigatória")]
        public string Senha { get; set; } = string.Empty;
    }
}
