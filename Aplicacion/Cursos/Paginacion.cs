using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Persistencia.DapperConexion.Paginacion;

namespace Aplicacion.Cursos
{
    public class Paginacion
    {
        public class Ejecuta: IRequest<PaginacionModel>{
            public string Titulo { get; set; }            
            public int NumeroPagina { get; set; }
            public int CantidadElementos { get; set; }
        }


        public class Handler : IRequestHandler<Ejecuta, PaginacionModel>
        {
            private readonly IPaginacion paginacion;

            public Handler(IPaginacion paginacion)
            {
                this.paginacion = paginacion;
            }

            public async Task<PaginacionModel> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var sp = "usp_curso_paginacion";
                var ordenamiento ="Titulo";
                
                var parametros = new Dictionary<string, object>();
                parametros.TryAdd("Titulo", request.Titulo);

                return await paginacion.DevolverPaginacion(sp, request.NumeroPagina, request.CantidadElementos, parametros,ordenamiento);
            }
        }
    }
}