using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Seguridad
{
    public class RolLista
    {
          public class Ejecuta: IRequest<List<IdentityRole>> {        
        }

        

        public class Handler : IRequestHandler<Ejecuta, List<IdentityRole>>
        {
            private readonly CursosOnlineContext context;

            public Handler(CursosOnlineContext context)
            {
                this.context = context;
            }

            public async Task<List<IdentityRole>> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                return await context.Roles.OrderBy(r => r.Name).ToListAsync();
            }            
        }
    }
}