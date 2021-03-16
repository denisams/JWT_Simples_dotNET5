using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JWT.app.Autenticacao
{
    public class AplicacaoContextoBD : IdentityDbContext<AplicacaoUsuario>
    {
        public AplicacaoContextoBD(DbContextOptions<AplicacaoContextoBD> options) : base(options)
        {
                
        }
    }
}
