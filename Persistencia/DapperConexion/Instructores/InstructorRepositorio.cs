using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;

namespace Persistencia.DapperConexion.Instructores
{
    public class InstructorRepositorio : IInstructor
    {
        private readonly IFactoryConnection factoryConnection;

        public InstructorRepositorio(IFactoryConnection factoryConnection)
        {
            this.factoryConnection = factoryConnection;
        }

        public async Task<int> Actualizar(InstructorModel param)
        {
            var sp = "usp_Editar_Instructor";
            int resultado = 0;
            try{
                var connection = factoryConnection.GetConnection();
                resultado = await connection.ExecuteAsync(sp, param, commandType:  CommandType.StoredProcedure);
            }catch(Exception ex){
                throw new Exception("No se pudo actualizar el nuevo instructor", ex);
            }finally{
                factoryConnection.CloseConnection();
            }
            return resultado;

        }

        public async Task<int> Eliminar(Guid id)
        {
             var sp = "usp_Eliminar_Instructor";
            int resultado = 0;
            try{
                var connection = factoryConnection.GetConnection();
                resultado = await connection.ExecuteAsync(sp, new {InstructorId = id}, commandType:  CommandType.StoredProcedure);
            }catch(Exception ex){
                throw new Exception("No se pudo eliminar el instructor", ex);
            }finally{
                factoryConnection.CloseConnection();
            }
            return resultado;
        }

        public async Task<int> Nuevo(string nombre, string apellidos, string grado)
        {
            var sp ="usp_Instructor_Nuevo";
            int resultado = 0;
            try{
                var connection = factoryConnection.GetConnection();
                resultado = await connection.ExecuteAsync(sp, new {
                        InstructorID = Guid.NewGuid(),
                        Nombre = nombre,
                        Apellidos = apellidos,
                        Grado = grado
                    }, commandType:  CommandType.StoredProcedure);
            }catch(Exception ex){
                throw new Exception("No se pudo guardar en nuevo instructor", ex);
            }finally{
                factoryConnection.CloseConnection();
            }
            return resultado;
        }

        public async Task<IEnumerable<InstructorModel>> ObtenerLista()
        {
            IEnumerable<InstructorModel> lista;
            var sp = "usp_Obtener_Instructores";
            try{
                var connection = factoryConnection.GetConnection();
                lista = await connection.QueryAsync<InstructorModel>(sp,null, commandType:CommandType.StoredProcedure);
            }catch(Exception ex){
                throw new Exception("Error en la consulta de datos", ex);
            }finally{
                factoryConnection.CloseConnection();
            }
            return lista;
        }

        public async Task<InstructorModel> ObtenerPorId(Guid id)
        {
              InstructorModel instructor;
            var sp = "usp_Obtener_InstructorPorId";
            try{
                var connection = factoryConnection.GetConnection();
                instructor = await connection.QuerySingleOrDefaultAsync<InstructorModel>(sp,new { InstructorId = id}, commandType:CommandType.StoredProcedure);
            }catch(Exception ex){
                throw new Exception("Error en la consulta de datos", ex);
            }finally{
                factoryConnection.CloseConnection();
            }
            return instructor;
        }
    }
}