using System.Linq;
using System.Security.Claims;
using Aplicacion.Contratos;
using Microsoft.AspNetCore.Http;

namespace Seguridad.TokenSeguridad
{
    public class UsuarioSesion: IUsuarioSesion
    {
        private readonly IHttpContextAccessor httpContext;

        public UsuarioSesion(IHttpContextAccessor httpContext)
        {
            this.httpContext = httpContext;
        }
        public string ObtenerUsuarioSesion()
        {
            return this.httpContext.HttpContext.User?.Claims?.FirstOrDefault(x => x.Type==ClaimTypes.NameIdentifier)?.Value;
        }
        
    }
}