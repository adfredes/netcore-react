using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using FluentValidation;
using MediatR;
using Persistencia.DapperConexion.Instructores;

namespace Aplicacion.Instructores
{
    public class Editar
    {
        public class Ejecuta: IRequest{
            public InstructorModel Instructor { get; set; }
        }

        public class EjecutaValida : AbstractValidator<Ejecuta>
        {
            public EjecutaValida()
            {
                RuleFor(x => x.Instructor.Apellidos).NotEmpty();
                RuleFor(x => x.Instructor.Nombre).NotEmpty();
                RuleFor(x => x.Instructor.Grado).NotEmpty();
                //RuleFor(x => x.Instructor.InstructorId).NotEmpty();
            }
        }


        public class Handler: IRequestHandler<Ejecuta, Unit>{
            private readonly IInstructor instructorRepository;
            public Handler(IInstructor instructorRepository)
            {
                this.instructorRepository = instructorRepository;
            }

            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var resultado = await instructorRepository.Actualizar(request.Instructor);
                if(resultado>0){
                    return Unit.Value;
                }

                throw new ManejadorExcepcion(System.Net.HttpStatusCode.BadRequest, "No se pudo actualizar el instructor");
            }
        }
    }
}