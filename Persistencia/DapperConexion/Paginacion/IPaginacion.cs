
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Persistencia.DapperConexion.Paginacion
{
    public interface IPaginacion
    {
        Task<PaginacionModel> DevolverPaginacion(string sp, int numeroPagina, 
                                                int cantidadElementos, 
                                                IDictionary<string, object> parametrosFiltro,
                                                string ordenamientoColumna);
    }
}