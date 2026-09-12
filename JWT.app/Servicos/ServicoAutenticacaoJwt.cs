using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JWT.app.Autenticacao;
using JWT.app.Opcoes;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace JWT.app.Servicos
{
    public class ServicoAutenticacaoJwt : IServicoAutenticacao
    {
        private readonly UserManager<AplicacaoUsuario> _gerenciadorUsuario;
        private readonly OpcoesJwt _opcoesJwt;

        public ServicoAutenticacaoJwt(UserManager<AplicacaoUsuario> gerenciadorUsuario, IOptions<OpcoesJwt> opcoesJwt)
        {
            _gerenciadorUsuario = gerenciadorUsuario;
            _opcoesJwt = opcoesJwt.Value;
        }

        public async Task<ResultadoAutenticacao?> AutenticarAsync(LoginModelo modelo)
        {
            var usuario = await _gerenciadorUsuario.FindByNameAsync(modelo.NomeUsuario);
            if (usuario is null || !await _gerenciadorUsuario.CheckPasswordAsync(usuario, modelo.Senha))
            {
                return null;
            }

            var papeisDoUsuario = await _gerenciadorUsuario.GetRolesAsync(usuario);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario.UserName!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };
            claims.AddRange(papeisDoUsuario.Select(papel => new Claim(ClaimTypes.Role, papel)));

            var chaveAssinatura = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_opcoesJwt.ChaveSecreta));

            var token = new JwtSecurityToken(
                issuer: _opcoesJwt.EmissorValido,
                audience: _opcoesJwt.AudienciaValida,
                expires: DateTime.Now.AddHours(_opcoesJwt.HorasParaExpirar),
                claims: claims,
                signingCredentials: new SigningCredentials(chaveAssinatura, SecurityAlgorithms.HmacSha256));

            return new ResultadoAutenticacao
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiracao = token.ValidTo
            };
        }

        public async Task<ResultadoCadastro> RegistrarAsync(CadastroModelo modelo)
        {
            var usuarioExistente = await _gerenciadorUsuario.FindByNameAsync(modelo.NomeUsuario);
            if (usuarioExistente is not null)
            {
                return new ResultadoCadastro { Sucesso = false, Mensagem = "Usuário já existe!" };
            }

            var usuario = new AplicacaoUsuario
            {
                Email = modelo.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = modelo.NomeUsuario
            };

            var resultado = await _gerenciadorUsuario.CreateAsync(usuario, modelo.Senha);
            if (!resultado.Succeeded)
            {
                var erros = string.Join(" ", resultado.Errors.Select(erro => erro.Description));
                return new ResultadoCadastro { Sucesso = false, Mensagem = $"Falha ao cadastrar: {erros}" };
            }

            return new ResultadoCadastro { Sucesso = true, Mensagem = "Usuário criado com sucesso!" };
        }
    }
}
