using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Dominio;
using FluentValidation;
using MediatR;
using Persistencia;

namespace Aplicacion.Cursos
{
    public class Nuevo
    {
        public class Ejecuta: IRequest {            
            // [Required(ErrorMessage="Ingrese el titulo")]
            public string Titulo { get; set; }            
            public string Descripcion { get; set; }            
            public DateTime? FechaPublicacion { get; set; }
            public List<Guid> ListaIntructor { get; set; }
            public decimal Precio{get;set;}
            public decimal Promocion { get; set; }
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
                var listaInstructor = new List<CursoInstructor>();
                request.ListaIntructor?.ForEach(instructor => {
                    var cursoInstrunctor = new CursoInstructor{                        
                        InstructorId = instructor
                    };
                    listaInstructor.Add(cursoInstrunctor);
                });

                var curso = new Curso{
                    CursoId = Guid.NewGuid(),
                    Titulo = request.Titulo,
                    Descripcion = request.Descripcion,
                    FechaPublicacion = request.FechaPublicacion.Value,
                    PrecioPromocion = new Precio{
                        PrecioId = Guid.NewGuid(),
                        PrecioActual = request.Precio,
                        Promocion = request.Promocion
                    },
                    InstructorLink = listaInstructor,
                    FechaCreacion = DateTime.UtcNow
                };

                
                    
                
                
                this.context.Curso.Add(curso);

                // if(request.ListaIntructor != null){
                //     request.ListaIntructor.ForEach(instructor => {
                //         var cursoInstrunctor = new CursoInstructor{
                //             CursoId = _cursoId,
                //             InstructorId = instructor
                //         };
                //         this.context.CursoInstructor.Add(cursoInstrunctor);
                //     });
                // }

                

                if(await this.context.SaveChangesAsync() > 0){
                    return Unit.Value;
                }else{
                    throw new Exception("No se pudo insertar el curso");
                }

            }
        }
    }
}