using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Dominio;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Cursos
{
    public class ConsultaId
    {
        public class CursoUnico: IRequest<CursoDto>{
            public Guid Id { get; set; }
        }
        public class Handler : IRequestHandler<CursoUnico, CursoDto>
        {
            private readonly CursosOnlineContext context;
            private readonly IMapper mapper;

            public Handler(CursosOnlineContext context, IMapper mapper)
            {
                this.context = context;
                this.mapper = mapper;
            }
            public async Task<CursoDto> Handle(CursoUnico request, CancellationToken cancellationToken)
            {
                var curso = await context.Curso
                                    .Where(c => c.CursoId == request.Id)
                                    .ProjectTo<CursoDto>(mapper.ConfigurationProvider)
                                    .FirstOrDefaultAsync();
                if(curso==null){
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new {mensaje = "No se encontro el curso"});
                }
                return curso;
            }
        }
    }
}