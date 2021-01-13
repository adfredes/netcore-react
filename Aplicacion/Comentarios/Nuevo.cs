using System;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using Dominio;
using FluentValidation;
using MediatR;
using Persistencia;

namespace Aplicacion.Comentarios
{
    public class Nuevo
    {
        public class Ejecuta: IRequest{
            public string Alumno { get; set; }
            public int Puntaje { get; set; }
            public string Comentario {get; set; }
            public Guid CursoId { get; set; }
        }

        public class ValidaEjecuta : AbstractValidator<Ejecuta>
        {
            public ValidaEjecuta()
            {
                RuleFor(x => x.Alumno).NotEmpty().NotNull();
                RuleFor(x => x.Puntaje).NotEmpty().NotNull();
                RuleFor(x => x.Comentario).NotEmpty().NotNull();
                RuleFor(x => x.CursoId).NotEmpty().NotNull();
            }
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
                var comentario = new Comentario{
                    Alumno = request.Alumno,
                    ComentarioId = Guid.NewGuid(),
                    ComentarioTexto = request.Comentario,
                    CursoId = request.CursoId,
                    Puntaje = request.Puntaje,
                    FechaCreacion = DateTime.UtcNow
                };

                context.Comentario.Add(comentario);

                if(await context.SaveChangesAsync()>0){
                    return Unit.Value;
                }

                throw new ManejadorExcepcion(System.Net.HttpStatusCode.BadRequest, "No se pudo agregar el comentario");
            }
        }
    }
}