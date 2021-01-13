using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using Dominio;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Cursos
{
    public class Editar
    {
        public class Ejecuta: IRequest{
            public Guid CursoId { get; set; }
            public string Titulo { get; set; }
            public string Descripcion { get; set; }
            public DateTime? FechaPublicacion { get; set; }
            public List<Guid> ListaInstructor { get; set; }
            public decimal? Precio{get;set;}
            public decimal? Promocion { get; set; }
        }

         public class EjecutaValidacion : AbstractValidator<Ejecuta>
        {
            public EjecutaValidacion()
            {
                RuleFor(x => x.Titulo).NotEmpty();
                RuleFor(x => x.Descripcion).NotEmpty();
                RuleFor(x => x.FechaPublicacion).NotEmpty();
                RuleFor(x => x.Precio).NotNull();
                RuleFor(x => x.Promocion).NotNull();
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
                var curso = await context.Curso
                    .Include(c => c.InstructorLink)
                    .Include(c => c.PrecioPromocion)
                    .FirstOrDefaultAsync(c=> c.CursoId == request.CursoId);

                if(curso == null){
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new {mensaje = "No se encontro el curso"});
                }                

                curso.Titulo = request.Titulo ?? curso.Titulo;
                curso.Descripcion = request.Descripcion ?? curso.Descripcion;
                curso.FechaPublicacion = request.FechaPublicacion ?? curso.FechaPublicacion;
                if(curso.PrecioPromocion==null){
                    curso.PrecioPromocion = new Precio{
                        PrecioId = Guid.NewGuid(),
                        PrecioActual = request.Precio ?? 0,
                        Promocion = request.Promocion ?? 0,
                    };
                }
                else{
                    curso.PrecioPromocion.PrecioActual = request.Precio ?? curso.PrecioPromocion.PrecioActual;
                    curso.PrecioPromocion.Promocion = request.Promocion ?? curso.PrecioPromocion.Promocion;
                }                

                /*eliminados*/                
                var eliminados = curso.InstructorLink.Where(i => !request.ListaInstructor.Contains(i.InstructorId)).ToList();
                eliminados.ForEach(e => curso.InstructorLink.Remove(e));                              

                /*agregados*/
                var agregados = request.ListaInstructor.Where(i => !curso.InstructorLink.Select(x=>x.InstructorId).Contains(i)).ToList();
                agregados.ForEach(i => curso.InstructorLink.Add(new CursoInstructor{
                    CursoId = curso.CursoId,
                    InstructorId = i
                }));
                
                
                

                if(await context.SaveChangesAsync() > 0){                    
                    return Unit.Value;
                }else{
                    throw new Exception("No se pudo actualizar el curso");
                }
            }
        }
    }
}