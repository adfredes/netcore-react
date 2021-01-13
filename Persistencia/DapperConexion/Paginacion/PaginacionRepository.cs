using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace Persistencia.DapperConexion.Paginacion
{
    public class PaginacionRepository : IPaginacion
    {
        private readonly IFactoryConnection factoryConnection;

        public PaginacionRepository(IFactoryConnection factoryConnection)
        {
            this.factoryConnection = factoryConnection;
        }

        public async Task<PaginacionModel> DevolverPaginacion(string sp, int numeroPagina, int cantidadElementos, IDictionary<string, object> parametrosFiltro, string ordenamientoColumna)
        {
            PaginacionModel paginacionModel = new PaginacionModel();
            List<IDictionary<string,object>> listaReporte = null;
            int totalRecords = 0;
            int totalPaginas = 0;
            try{
                var connection = factoryConnection.GetConnection();       
                DynamicParameters parametros = new DynamicParameters()         ;
                
                parametros.Add("@NumeroPagina", numeroPagina);
                parametros.Add("@CantidadElementos", cantidadElementos);
                parametros.Add("@Ordenamiento", ordenamientoColumna);

                parametros.Add("@TotalRecords", totalRecords, DbType.Int32, ParameterDirection.Output);
                parametros.Add("@TotalPaginas", totalPaginas, DbType.Int32, ParameterDirection.Output);

                foreach (var param in parametrosFiltro)
                {
                    parametros.Add($"@{param.Key}", param.Value);
                }

                var result = await connection.QueryAsync(sp, parametros, commandType: CommandType.StoredProcedure);
                listaReporte = result.Select(x =>  (IDictionary<string,object>)x).ToList();
                paginacionModel.ListaRecords = listaReporte;
                paginacionModel.NumeroPaginas = parametros.Get<int>("@TotalPaginas");
                paginacionModel.TotalRecords = parametros.Get<int>("@TotalRecords");

            }catch(Exception ex){
                throw new Exception("No se pudo ejecutar el procedimiento almacenado", ex);
            }finally{
                factoryConnection.CloseConnection();
            }

            return paginacionModel;
        }
    }
}