using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.Contratos;
using Aplicacion.ManejadorError;
using Dominio;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Aplicacion.Seguridad
{
    public class Login
    {
        public class Ejecuta : IRequest<UsuarioData> {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class EjecutaValidacion : AbstractValidator<Ejecuta>
        {
            public EjecutaValidacion()
            {
                RuleFor(x => x.Email).NotEmpty();
                RuleFor(x => x.Password).NotEmpty();                
            }
        }

        public class Handler : IRequestHandler<Ejecuta, UsuarioData>
        {
            private readonly UserManager<Usuario> userManager;
            private readonly SignInManager<Usuario> signInManager;
            private readonly IJwtGenerador jwtGenerador;

            public Handler(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager, IJwtGenerador jwtGenerador)
            {
                this.userManager = userManager;
                this.signInManager = signInManager;
                this.jwtGenerador = jwtGenerador;
            }
            public async Task<UsuarioData> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var usuario = await this.userManager.FindByEmailAsync(request.Email);
                if(usuario == null){
                    throw new ManejadorExcepcion(HttpStatusCode.Unauthorized);
                }

                var result = await signInManager.CheckPasswordSignInAsync(usuario,request.Password, false);

                var roles = (await userManager.GetRolesAsync(usuario)).ToList();

                if(result.Succeeded){
                    return new UsuarioData{
                        Email = usuario.Email,
                        Imagen = null,
                        NombreCompleto = usuario.NombreCompleto,
                        Username = usuario.UserName,
                        Token = jwtGenerador.CrearToken(usuario, roles)
                    };
                }

                throw new ManejadorExcepcion(HttpStatusCode.Unauthorized);
            }
        }
    }
}