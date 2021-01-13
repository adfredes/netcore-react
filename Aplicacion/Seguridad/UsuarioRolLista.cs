using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dominio;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Aplicacion.Seguridad
{
    public class UsuarioRolLista
    {
        public class Ejecuta: IRequest <List<string>> {
            public string Username { get; set; }            
        }

        public class ValidaEjecuta : AbstractValidator<Ejecuta>
        {
            public ValidaEjecuta()
            {
                RuleFor(x => x.Username).NotEmpty();                
            }
        }

        public class Handler : IRequestHandler<Ejecuta, List<string>>
        {
            private readonly RoleManager<IdentityRole> roleManager;
            private readonly UserManager<Usuario> userManager;

            public Handler(RoleManager<IdentityRole> roleManager, UserManager<Usuario> userManager)
            {
                this.roleManager = roleManager;
                this.userManager = userManager;
            }            

            public async Task<List<string>> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var usuario = await userManager.FindByNameAsync(request.Username);
                if(usuario==null){
                    throw new ManejadorError.ManejadorExcepcion(System.Net.HttpStatusCode.NotFound,"No se encontro el usuario");
                }

                var roles = await userManager.GetRolesAsync(usuario);
                return roles.ToList();                            
            }
        }
    }
}