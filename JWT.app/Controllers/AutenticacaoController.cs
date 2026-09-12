using JWT.app.Autenticacao;
using JWT.app.Servicos;
using Microsoft.AspNetCore.Mvc;

namespace JWT.app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutenticacaoController : ControllerBase
    {
        private readonly IServicoAutenticacao _servicoAutenticacao;

        public AutenticacaoController(IServicoAutenticacao servicoAutenticacao)
        {
            _servicoAutenticacao = servicoAutenticacao;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModelo modelo)
        {
            var resultado = await _servicoAutenticacao.AutenticarAsync(modelo);
            if (resultado is null)
            {
                return Unauthorized();
            }

            return Ok(resultado);
        }

        [HttpPost("cadastro")]
        public async Task<IActionResult> Cadastrar([FromBody] CadastroModelo modelo)
        {
            var resultado = await _servicoAutenticacao.RegistrarAsync(modelo);
            if (!resultado.Sucesso)
            {
                return BadRequest(new Resposta { Status = "Erro", Mensagem = resultado.Mensagem });
            }

            return Ok(new Resposta { Status = "Sucesso", Mensagem = resultado.Mensagem });
        }
    }
}
