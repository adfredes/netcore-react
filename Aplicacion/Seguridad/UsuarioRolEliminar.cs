using System.Threading;
using System.Threading.Tasks;
using Dominio;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Aplicacion.Seguridad
{
    public class UsuarioRolEliminar
    {
             public class Ejecuta: IRequest {
            public string Username { get; set; }
            public string RoleNombre { get; set; }
        }

        public class ValidaEjecuta : AbstractValidator<Ejecuta>
        {
            public ValidaEjecuta()
            {
                RuleFor(x => x.Username).NotEmpty();
                RuleFor(x => x.RoleNombre).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Ejecuta>
        {
            private readonly RoleManager<IdentityRole> roleManager;
            private readonly UserManager<Usuario> userManager;

            public Handler(RoleManager<IdentityRole> roleManager, UserManager<Usuario> userManager)
            {
                this.roleManager = roleManager;
                this.userManager = userManager;
            }            

            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var usuario = await userManager.FindByNameAsync(request.Username);
                if(usuario==null){
                    throw new ManejadorError.ManejadorExcepcion(System.Net.HttpStatusCode.NotFound,"No se encontro el usuario");
                }
                
                if(!await roleManager.RoleExistsAsync(request.RoleNombre)){
                    throw new ManejadorError.ManejadorExcepcion(System.Net.HttpStatusCode.NotFound,"No se encontro el role");
                }

                var result = await userManager.RemoveFromRoleAsync(usuario, request.RoleNombre);
                if(result.Succeeded){
                    return Unit.Value;
                }

                throw new System.Exception("No pudo eliminar el rol al usuario");
            }
        }

       
    }
}