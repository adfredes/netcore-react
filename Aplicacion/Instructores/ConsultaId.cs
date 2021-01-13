using System;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using MediatR;
using Persistencia.DapperConexion.Instructores;

namespace Aplicacion.Instructores
{
    public class ConsultaId
    {
        public class InstructorUnico: IRequest<InstructorModel>{
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<InstructorUnico, InstructorModel>
        {
            private readonly IInstructor instructorRepository;

            public Handler(IInstructor instructorRepository)
            {
                this.instructorRepository = instructorRepository;
            }

            public async Task<InstructorModel> Handle(InstructorUnico request, CancellationToken cancellationToken)
            {
                var instructor = await instructorRepository.ObtenerPorId(request.Id);
                if(instructor == null) throw new ManejadorExcepcion(System.Net.HttpStatusCode.NotFound,"No se encontro el instructo");
                return instructor;
            }
        }
    }
}