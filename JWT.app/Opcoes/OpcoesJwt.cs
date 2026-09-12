namespace JWT.app.Opcoes
{
    public class OpcoesJwt
    {
        public const string Secao = "JWT";

        public string ChaveSecreta { get; set; } = string.Empty;
        public string EmissorValido { get; set; } = string.Empty;
        public string AudienciaValida { get; set; } = string.Empty;
        public int HorasParaExpirar { get; set; } = 3;
    }
}
