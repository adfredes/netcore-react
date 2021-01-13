using System.Threading.Tasks;
using Aplicacion.Seguridad;
using Dominio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [AllowAnonymous]
    public class UsuarioController: MiControllerBase
    {
        [HttpPost("login")]        
        public async Task<ActionResult<UsuarioData>> Login(Login.Ejecuta data){
            return await mediator.Send(data);
        }

        [HttpPost("registrar")]
        public async Task<ActionResult<UsuarioData>> Registrar(Registrar.Ejecuta data){
            return await mediator.Send(data);
        }
        [HttpGet]
        public async Task<ActionResult<UsuarioData>> DevolverUsuario(){
            return await mediator.Send(new UsuarioActual.Ejecutar());
        }

        [HttpPut]
        public async Task<ActionResult<UsuarioData>> Actualizar(UsuarioActualizar.Ejecuta data){
            return await mediator.Send(data);
        }
    }
}