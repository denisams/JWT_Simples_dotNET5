using JWT.app.Autenticacao;

namespace JWT.app.Servicos
{
    public interface IServicoAutenticacao
    {
        Task<ResultadoAutenticacao?> AutenticarAsync(LoginModelo modelo);
        Task<ResultadoCadastro> RegistrarAsync(CadastroModelo modelo);
    }
}
