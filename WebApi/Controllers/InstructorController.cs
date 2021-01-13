using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aplicacion.Instructores;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Persistencia.DapperConexion.Instructores;

namespace WebApi.Controllers
{
    public class InstructorController: MiControllerBase
    {
        [Authorize(Roles="Administrador")]
        [HttpGet]
        public async Task<ActionResult<List<InstructorModel>>> ObtenerInstructores(){
            return await mediator.Send(new Consulta.Lista());
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<InstructorModel>> ObtenerInstructor(Guid id){            
            return await mediator.Send(new ConsultaId.InstructorUnico{Id=id});
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> Crear(Nuevo.Ejecuta data){
            return await mediator.Send(data);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> Modificar(Guid id, Editar.Ejecuta data){
            data.Instructor.InstructorId = id;
            return await mediator.Send(data);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Eliminar(Guid id){            
            return await mediator.Send(new Eliminar.Ejecuta{id=id});
        }
    }
}