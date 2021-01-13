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
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Seguridad
{
    public class Registrar
    {
        public class Ejecuta : IRequest<UsuarioData>
        {
            public string Nombre { get; set; }
            public string Apellidos { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string Username { get; set; }
        }

         public class EjecutaValidacion : AbstractValidator<Ejecuta>
        {
            public EjecutaValidacion()
            {
                RuleFor(x => x.Email).NotEmpty();
                RuleFor(x => x.Password).NotEmpty();                
                RuleFor(x => x.Nombre).NotEmpty();
                RuleFor(x => x.Apellidos).NotEmpty();
                RuleFor(x => x.Password).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Ejecuta, UsuarioData>
        {
            private readonly UserManager<Usuario> userManager;
            private readonly IJwtGenerador jwtGenerador;
            private readonly CursosOnlineContext context;

            public Handler(CursosOnlineContext context, UserManager<Usuario> userManager, IJwtGenerador jwtGenerador)
            {
                this.context = context;
                this.userManager = userManager;
                this.jwtGenerador = jwtGenerador;
            }

            public async Task<UsuarioData> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                
                if(await context.Users.Where(u => u.Email == request.Email).AnyAsync()){
                    throw new ManejadorExcepcion(HttpStatusCode.BadRequest, new {mensaje = "El mail ingresado ya existe"});
                }

                if(await context.Users.Where(u => u.UserName == request.Username).AnyAsync()){
                    throw new ManejadorExcepcion(HttpStatusCode.BadRequest, new {mensaje = "El usuario ingresado ya existe"});
                }
                var usuario = new Usuario
                {
                    NombreCompleto = request.Nombre + ' ' + request.Apellidos,
                    Email = request.Email,
                    UserName = request.Username
                };

                var result = await userManager.CreateAsync(usuario, request.Password);
                if(result.Succeeded){
                    return new UsuarioData{
                        Email = usuario.Email,
                        Imagen = null,
                        NombreCompleto = usuario.NombreCompleto,
                        Username = usuario.UserName,
                        Token = jwtGenerador.CrearToken(usuario, null)
                    };
                }
                throw new ManejadorExcepcion(HttpStatusCode.BadRequest, new {mensaje = "El mail ingresado ya existe"});
            }
        }
    }
}