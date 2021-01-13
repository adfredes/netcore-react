using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Persistencia.DapperConexion.Instructores;

namespace Aplicacion.Instructores
{
    public class Consulta
    {
        public class Lista: IRequest<List<InstructorModel>>{
            
        }

        public class Handler : IRequestHandler<Lista, List<InstructorModel>>
        {
            private readonly IInstructor instructorRepository;

            public Handler(IInstructor instructorRepository)
            {
                this.instructorRepository = instructorRepository;
            }

            public async Task<List<InstructorModel>> Handle(Lista request, CancellationToken cancellationToken)
            {                
                return (await instructorRepository.ObtenerLista()).ToList();
            }
        }
    }
}