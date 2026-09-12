namespace JWT.app.Autenticacao
{
    public class ResultadoAutenticacao
    {
        public string Token { get; set; } = string.Empty;
        public DateTime Expiracao { get; set; }
    }
}
