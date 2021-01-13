using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dominio;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Persistencia;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly CursosOnlineContext context;

        public WeatherForecastController(CursosOnlineContext context){
            this.context = context;
        }

      [HttpGet]
      public IEnumerable<Curso> GetVs(){
          return context.Curso.ToList();
      }
    }
}
