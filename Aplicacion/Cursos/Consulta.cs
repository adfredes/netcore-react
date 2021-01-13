using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Dominio;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Cursos
{
    public class Consulta
    {
        //lo que va a devolver
        public class ListaCursos : IRequest<List<CursoDto>> {}

        public class Handler : IRequestHandler<ListaCursos, List<CursoDto>>
        {
            private readonly CursosOnlineContext context;
            private readonly IMapper mapper;

            public Handler(CursosOnlineContext context, IMapper mapper)
            {
                this.context = context;
                this.mapper = mapper;
            }

            public async Task<List<CursoDto>> Handle(ListaCursos request, CancellationToken cancellationToken)
            {
                var cursos = await context.Curso
                    // .Include(x => x.InstructorLink)
                    //     .ThenInclude(i => i.Instructor)
                    .ProjectTo<CursoDto>(mapper.ConfigurationProvider)                    
                    .ToListAsync();                
                return cursos;
            }

        }
    }
}