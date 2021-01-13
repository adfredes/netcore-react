using System;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using MediatR;
using Persistencia.DapperConexion.Instructores;

namespace Aplicacion.Instructores
{
    public class Eliminar
    {
        public class Ejecuta:IRequest{
            public Guid id { get; set; }
        }

        public class Handler: IRequestHandler<Ejecuta, Unit>{
            private readonly IInstructor instructorRepository;
            public Handler(IInstructor instructorRepository)
            {
                this.instructorRepository = instructorRepository;
            }

            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {                
                var resultado = await instructorRepository.Eliminar(request.id);
                if(resultado > 0){
                    return Unit.Value;
                }
                
                throw new ManejadorExcepcion(System.Net.HttpStatusCode.BadRequest,"No se pudo eliminar el instructor");
            }
        }
    }
}