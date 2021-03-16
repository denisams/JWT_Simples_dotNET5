using JWT.app.Autenticacao;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JWT.app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutenticacaoController : ControllerBase
    {
        private readonly UserManager<AplicacaoUsuario> gerenciadorUsuario;
        private readonly IConfiguration _configuracao;

        public AutenticacaoController(UserManager<AplicacaoUsuario> gerenciadorUsuario, IConfiguration configuracao)
        {
            this.gerenciadorUsuario = gerenciadorUsuario;
            _configuracao = configuracao;
        }


        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModelo model)
        {
            var user = await gerenciadorUsuario.FindByNameAsync(model.NomeUsuario);
            if (user != null && await gerenciadorUsuario.CheckPasswordAsync(user, model.Senha))
            {
                var userRoles = await gerenciadorUsuario.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuracao["JWT:Secret"]));

                var token = new JwtSecurityToken(
                    issuer: _configuracao["JWT:ValidIssuer"],
                    audience: _configuracao["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }
            return Unauthorized();
        }


        [HttpPost]
        [Route("cadastro")]
        public async Task<IActionResult> Register([FromBody] CadastroModelo model)
        {
            var userExists = await gerenciadorUsuario.FindByNameAsync(model.NomeUsuario);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Resposta { Status = "Erro", Mensagem = "Usuário já existe!" });

            AplicacaoUsuario user = new AplicacaoUsuario()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.NomeUsuario
            };
            var result = await gerenciadorUsuario.CreateAsync(user, model.Senha);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Resposta { Status = "Erro", Mensagem = "Falha! Favor tentar cadastrar novamente." });

            return Ok(new Resposta { Status = "Sucesso", Mensagem = "Usuário criado com sucesso!" });
        }

    }
}
