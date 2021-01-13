using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.Contratos;
using Dominio;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Seguridad
{
    public class UsuarioActualizar
    {
        public class Ejecuta: IRequest<UsuarioData>{
            public string Nombre { get; set; }
            public string Apellidos { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string Username { get; set; }
        }

        public class EjecutaValida : AbstractValidator<Ejecuta>
        {
            public EjecutaValida()
            {
                RuleFor(x=>x.Nombre).NotEmpty();
                RuleFor(x=>x.Apellidos).NotEmpty();
                RuleFor(x=>x.Email).NotEmpty();
                RuleFor(x=>x.Password).NotEmpty();
                RuleFor(x=>x.Username).NotEmpty();                
            }
        }

        public class Handler : IRequestHandler<Ejecuta, UsuarioData>
        {
            private readonly CursosOnlineContext context;
            private readonly UserManager<Usuario> userManager;
            private readonly IJwtGenerador jwtGenerador;
            private readonly IPasswordHasher<Usuario> passwordHasher;

            public Handler(CursosOnlineContext context, UserManager<Usuario> userManager, IJwtGenerador jwtGenerador, IPasswordHasher<Usuario> passwordHasher)
            {
                this.context = context;
                this.userManager = userManager;
                this.jwtGenerador = jwtGenerador;
                this.passwordHasher = passwordHasher;
            }

            public async Task<UsuarioData> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var usuarioIden = await userManager.FindByNameAsync(request.Username);
                if(usuarioIden==null) throw new ManejadorError.ManejadorExcepcion(System.Net.HttpStatusCode.NotFound, "El usuario no existe");
                
                var existeMail = await context.Users
                                                .Where(u => u.Email == request.Email && u.UserName != request.Username)
                                                .AnyAsync();
                if(existeMail) throw new ManejadorError.ManejadorExcepcion(System.Net.HttpStatusCode.NotFound, "El mail ya esta registrado por otro usuario");
                
                //var usuario = await context.Users.Where(u => u.UserName == request.Username).FirstOrDefaultAsync();

                usuarioIden.NombreCompleto = $"{request.Nombre} {request.Apellidos}";
                usuarioIden.PasswordHash = passwordHasher.HashPassword(usuarioIden, request.Password);
                usuarioIden.Email = request.Email;

                var result = await userManager.UpdateAsync(usuarioIden);

                if(result.Succeeded){
                    var roles = (await userManager.GetRolesAsync(usuarioIden)).ToList();
                    return new UsuarioData{
                        Email= usuarioIden.Email,
                        Username = usuarioIden.UserName,
                        NombreCompleto = usuarioIden.NombreCompleto,
                        Token = jwtGenerador.CrearToken(usuarioIden,roles)
                    };
                }

                throw new System.Exception("Error al modificar los datos del usuario");
            }
        }
    }
}