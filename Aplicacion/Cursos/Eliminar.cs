using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Cursos
{
    public class Eliminar
    {
        public class Ejecuta: IRequest{
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Ejecuta>
        {
            private readonly CursosOnlineContext context;
            public Handler(CursosOnlineContext context)
            {
                this.context = context;
            }
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var curso = await context.Curso                                    
                                    .Where(c => c.CursoId == request.Id)
                                    .FirstOrDefaultAsync();

                if(curso == null){                    
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new {mensaje = "No se encontro el curso"});
                }

                context.Curso.Remove(curso);

                 if(await context.SaveChangesAsync() > 0){
                    return Unit.Value;
                }else{
                    throw new ManejadorExcepcion(HttpStatusCode.BadRequest, new {mensaje = "No se pudo eliminar el curso"});
                }

            }
        }
    }
}