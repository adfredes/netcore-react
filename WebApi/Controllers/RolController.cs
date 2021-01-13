using System.Collections.Generic;
using System.Threading.Tasks;
using Aplicacion.Seguridad;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    public class RolController: MiControllerBase
    {
        [HttpPost("crear")]
        public async Task<ActionResult<Unit>> Crear(RolNuevo.Ejecuta data){
            return await mediator.Send(data);
        }

        [HttpDelete("eliminar")]
        public async Task<ActionResult<Unit>> Eliminar(RolEliminar.Ejecuta data){
            return await mediator.Send(data);
        }

        [HttpGet("lista")]
        public async Task<ActionResult<List<IdentityRole>>> Listar(){
            return await mediator.Send(new RolLista.Ejecuta());
        }

        [HttpPost("agregarRoleUsuario")]
        public async Task<ActionResult<Unit>> AgregarRoleUsuario(UsuarioRolAgregar.Ejecuta data){
            return await mediator.Send(data);
        }

        [HttpDelete("eliminarRoleUsuario")]
        public async Task<ActionResult<Unit>> EliminarRoleUsuario(UsuarioRolEliminar.Ejecuta data){
            return await mediator.Send(data);
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<List<string>>> ListaRoleUsuario(string username){
            return await mediator.Send(new UsuarioRolLista.Ejecuta{Username=username});
        }
    }
}