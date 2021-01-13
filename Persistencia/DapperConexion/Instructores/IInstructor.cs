using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Persistencia.DapperConexion.Instructores
{
    public interface IInstructor
    {
        Task<IEnumerable<InstructorModel>> ObtenerLista();
        Task<InstructorModel> ObtenerPorId(Guid id);
        Task<int> Nuevo(string nombre, string apellidos, string grado);
        Task<int> Actualizar(InstructorModel parametros);
        Task<int> Eliminar(Guid id);
    }
}