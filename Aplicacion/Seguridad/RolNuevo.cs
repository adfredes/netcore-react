using System;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Aplicacion.Seguridad
{
    public class RolNuevo
    {
        public class Ejecuta: IRequest {
            public string Nombre { get; set; }
        }

        public class ValidaEjecuta : AbstractValidator<Ejecuta>
        {
            public ValidaEjecuta()
            {
                RuleFor(x => x.Nombre).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Ejecuta>
        {
            private readonly RoleManager<IdentityRole> roleManager;

            public Handler(RoleManager<IdentityRole> roleManager)
            {
                this.roleManager = roleManager;
            }

            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var role = await roleManager.FindByNameAsync(request.Nombre);
                if(role!=null){
                    throw new ManejadorExcepcion(System.Net.HttpStatusCode.BadRequest, new {mensaje = "El role ya existe"});
                }
                var result = await roleManager.CreateAsync(new IdentityRole(request.Nombre));

                if(result.Succeeded){
                    return Unit.Value;
                }
                
                throw new Exception("No se pudo guardar el rol");
            }
        }
    }
}