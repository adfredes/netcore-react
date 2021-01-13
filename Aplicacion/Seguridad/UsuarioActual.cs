using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.Contratos;
using Dominio;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Aplicacion.Seguridad
{
    public class UsuarioActual
    {
        public class Ejecutar :IRequest<UsuarioData> {}

        public class Handler : IRequestHandler<Ejecutar, UsuarioData>
        {
            private readonly UserManager<Usuario> userManager;
            private readonly IJwtGenerador jwtGenerador;
            private readonly IUsuarioSesion usuarioSesion;

            public Handler(UserManager<Usuario> userManager, IJwtGenerador jwtGenerador, IUsuarioSesion usuarioSesion)
            {
                this.userManager = userManager;
                this.jwtGenerador = jwtGenerador;
                this.usuarioSesion = usuarioSesion;
            }

            public async Task<UsuarioData> Handle(Ejecutar request, CancellationToken cancellationToken)
            {
                var usuario = await userManager.FindByNameAsync(usuarioSesion.ObtenerUsuarioSesion());
                var roles = (await userManager.GetRolesAsync(usuario)).ToList();
            return new UsuarioData{
                        Email = usuario.Email,
                        Imagen = null,
                        NombreCompleto = usuario.NombreCompleto,
                        Username = usuario.UserName,
                        Token = jwtGenerador.CrearToken(usuario, roles)
                    };
            }
        }
    }
}