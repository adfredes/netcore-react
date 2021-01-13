using System;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Comentarios
{
    public class Eliminar
    {
        public class Ejecuta: IRequest{            
            public Guid ComentarioId { get; set; }
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
                var comentario = await context.Comentario.FirstOrDefaultAsync(c => c.ComentarioId == request.ComentarioId);

                if(comentario == null) throw new ManejadorExcepcion(System.Net.HttpStatusCode.NotFound, "El comentario no existe");
                
                context.Comentario.Remove(comentario);

                if(await context.SaveChangesAsync()>0){
                    return Unit.Value;
                }

                throw new ManejadorExcepcion(System.Net.HttpStatusCode.BadRequest, "No se pudo eliminar el comentario");
            }
            
        }
    }
}