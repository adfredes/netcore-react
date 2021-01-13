using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using FluentValidation;
using MediatR;
using Persistencia.DapperConexion.Instructores;

namespace Aplicacion.Instructores
{
    public class Nuevo
    {
        public class Ejecuta: IRequest{            
            public string Nombre { get; set; }
            public string Apellidos { get; set; }
            public string Grado { get; set; }
        }

        public class EjecutaValida: AbstractValidator<Ejecuta>{
            public EjecutaValida(){
                RuleFor(x => x.Apellidos).NotEmpty();
                RuleFor(x => x.Nombre).NotEmpty();
                RuleFor(x => x.Grado).NotEmpty();

            }
        }

        public class Handler : IRequestHandler<Ejecuta, Unit>
        {
            private readonly IInstructor instructorRepository;

            public Handler(IInstructor instructorRepository)
            {
                this.instructorRepository = instructorRepository;
            }

            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var resultado =  await instructorRepository.Nuevo(request.Nombre, request.Apellidos, request.Grado);
                if(resultado > 0){
                    return Unit.Value;
                }

                throw new ManejadorExcepcion(System.Net.HttpStatusCode.BadRequest,"Error al agregar el instructor");

            }
        }
    }
}