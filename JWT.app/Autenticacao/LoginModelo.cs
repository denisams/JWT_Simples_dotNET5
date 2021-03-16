using System.ComponentModel.DataAnnotations;

namespace JWT.app.Autenticacao
{
    public class LoginModelo
    {
        [Required(ErrorMessage = "Nome é obrigatório")]
        public string NomeUsuario { get; set; }

        [Required(ErrorMessage = "Senha é obrigatória")]
        public string Senha { get; set; }
    }
}
