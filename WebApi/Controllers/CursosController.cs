using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aplicacion.Cursos;
using Dominio;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Persistencia.DapperConexion.Paginacion;

namespace WebApi.Controllers
{    
    public class CursosController: MiControllerBase
    {        

        [HttpGet]        
        public async Task<ActionResult<List<CursoDto>>> Get(){
            return await this.mediator.Send(new Consulta.ListaCursos());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CursoDto>> GetById(Guid id){
            return await this.mediator.Send(new ConsultaId.CursoUnico{Id = id});
        }

        [HttpPost("report")]
        public async Task<ActionResult<PaginacionModel>> Report(Paginacion.Ejecuta data){
            return await this.mediator.Send(data);
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> Crear(Nuevo.Ejecuta data){
            return await this.mediator.Send(data);            
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> Actualizar(Guid id, Editar.Ejecuta data){
            data.CursoId = id;
            return await this.mediator.Send(data);
        }

        [HttpDelete]
        public async Task<ActionResult<Unit>> Borrar(Eliminar.Ejecuta data){
            return await this.mediator.Send(data);
        }
    }
}