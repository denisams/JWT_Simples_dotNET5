# JWT_Simples_dotNET5

API .NET 8 de exemplo demonstrando autenticação com JWT usando ASP.NET Core Identity + Entity Framework Core (SQL Server).

## Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQL Server ou LocalDB (o padrão em `appsettings.json` aponta para `(localdb)\MSSQLLocalDB`)

## Configurando a chave JWT

Por segurança, **não** deixe uma chave secreta real em `appsettings.json`. Configure-a via [user-secrets](https://learn.microsoft.com/aspnet/core/security/app-secrets) em desenvolvimento:

```
cd JWT.app
dotnet user-secrets set "JWT:ChaveSecreta" "uma-chave-bem-grande-e-aleatoria-com-32-ou-mais-caracteres"
```

Em produção, defina via variável de ambiente (`JWT__ChaveSecreta`) ou outro cofre de segredos (Azure Key Vault, etc).

## String de conexão

A string de conexão fica em `appsettings.json`, seção `ConnectionStrings:ConnStr`. Ajuste para o seu servidor/instância antes de rodar as migrations.

## Rodando as migrations

Com o [dotnet-ef](https://learn.microsoft.com/ef/core/cli/dotnet) instalado (`dotnet tool install --global dotnet-ef`):

```
cd JWT.app
dotnet ef database update
```

## Rodando a API

```
cd JWT.app
dotnet run
```

O Swagger fica disponível em `/swagger` no ambiente de Desenvolvimento.

## Arquitetura

- `Controllers/AutenticacaoController.cs` — endpoints HTTP (`login`, `cadastro`), sem lógica de autenticação.
- `Servicos/IServicoAutenticacao.cs` e `Servicos/ServicoAutenticacaoJwt.cs` — geração de token e cadastro de usuário, isolando `UserManager`/JWT do controller.
- `Opcoes/OpcoesJwt.cs` — configuração do JWT (emissor, audiência, chave secreta), vinculada à seção `JWT` do `appsettings.json` via `IOptions`.
- `Autenticacao/` — modelos de entrada (`LoginModelo`, `CadastroModelo`), de saída (`ResultadoAutenticacao`, `ResultadoCadastro`, `Resposta`) e o `DbContext`/`AplicacaoUsuario` do Identity.

## Testando a API

Cadastro:

```
curl --location --request POST 'https://localhost:5001/api/Autenticacao/cadastro' \
--header 'Content-Type: application/json' \
--data-raw '{"nomeUsuario":"usuario1","email":"usuario1@teste.com","senha":"Senha@123"}'
```

Login:

```
curl --location --request POST 'https://localhost:5001/api/Autenticacao/login' \
--header 'Content-Type: application/json' \
--data-raw '{"nomeUsuario":"usuario1","senha":"Senha@123"}'
```

A resposta do login traz `token` e `expiracao`. Use o token no header `Authorization: Bearer <token>` para acessar endpoints protegidos com `[Authorize]`.

> Se estiver rodando via IIS Express ou em outra porta, ajuste a URL conforme `Properties/launchSettings.json`.
