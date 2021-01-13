using System;
using System.Threading.Tasks;
using Aplicacion.Comentarios;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    public class ComentarioController: MiControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<Unit>> Agregar(Nuevo.Ejecuta data){
            return await mediator.Send(data);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Eliminar(Guid id){
            return await mediator.Send(new Eliminar.Ejecuta{ComentarioId = id});
        }
    }
}