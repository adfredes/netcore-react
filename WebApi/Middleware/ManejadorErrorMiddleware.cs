using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace WebApi.Middleware
{
    public class ManejadorErrorMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ManejadorErrorMiddleware> logger;

        public ManejadorErrorMiddleware(RequestDelegate next, ILogger<ManejadorErrorMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task Invoke(HttpContext context) {
            try{
                await next(context);           
            }catch(Exception ex){
                await ManejadorExcepcionAsync(context, ex, logger);
            }            
        }

        private async Task ManejadorExcepcionAsync(HttpContext context, Exception ex, ILogger<ManejadorErrorMiddleware> _logger){
            object errores = null;            
            switch(ex){
                case ManejadorExcepcion me:
                    _logger.LogError(ex, "Manejador Error");
                    errores = me.Errores;
                    context.Response.StatusCode = (int)me.Codigo;
                    break;
                case Exception e:
                    _logger.LogError(ex, "Error de servidor");
                    errores = string.IsNullOrWhiteSpace(e.Message) ? "Error" : e.Message;
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            context.Response.ContentType = "application/json";
            if(errores != null){
                var resultados = JsonSerializer.Serialize(new{errores});
                await context.Response.WriteAsync(resultados);
                //await context.Response.WriteAsJsonAsync(new{errores});
            }
        }
    }
}